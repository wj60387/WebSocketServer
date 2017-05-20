using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperSocket.SocketBase.Logging;
using SuperWebSocket;
using log4net;
using ProtocalData.Utilities;
using ProtocalData.Protocol;
using ProtocalData.Protocol.Derive;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using Server.MessageHandleCenter;
using Server.Security;
using Newtonsoft.Json;
namespace Server
{
    public partial class FrmMain : Form
    {
        public   Dictionary<string, string> Mac_SN
        {
            get
            {
                var dict=new Dictionary<string, string>();

                foreach (WebSocketSession session in appServer.GetAllSessions())
                {
                    dict.Add(session.Cookies["MAC"], session.Cookies["SN"]);
                } 
                return dict;
            }
        }
        WebSocketServer  appServer = new WebSocketServer();
        ServerConfig serverConfig = new ServerConfig
            {
                Port = 2014,//set the listening port
                MaxConnectionNumber = 10000,
                MaxRequestLength = 5*1024 * 1024,
            };
        public FrmMain()
        {
            InitializeComponent();
             
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (!appServer.Setup(serverConfig)) //Setup the appServer
            {
                System.Windows.Forms.MessageBox.Show("开启服务器失败");
                return;
            }

            if (!appServer.Start())//Try to start the appServer
            {
                System.Windows.Forms.MessageBox.Show("开启服务器失败");
                return;
            }
            //注册事件

            appServer.NewSessionConnected += appServer_NewSessionConnected;//客户端连接
            appServer.NewMessageReceived += appServer_NewMessageReceived;//客户端接收消息
            appServer.NewDataReceived += appServer_NewDataReceived;
            appServer.SessionClosed += appServer_SessionClosed;//客户端关闭
            var host = new System.ServiceModel.ServiceHost(typeof(AuscultationService.AuscultationService)
                , new Uri(@"http://localhost/Query")
                );
            host.Open();
            this.FormClosing += (s, arg) => { host.Close(); };

            Load_dgvAuthInfo();
        }
        void Load_dgvAuthInfo()
        {
            dgvAuthInfo.Rows.Clear();
            string sql = "select * from AccountAuthCustomInfo";
            var ds = SqlHelper.ExecuteDataset(Setting.connectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dgvAuthInfo.Rows.Add(
                        null,
                        null,
                        row[0],
                        row[1],
                        row[2],
                        row[3],
                        row[4],
                        row[5],
                        row[6],
                        row[7],
                        row[8],
                        row[9],
                        row[10],
                        row[11],
                        row[12],
                        row[13]
                        );
                }
            }

        }




        void appServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            
           var code= SerializaHelper.DeSerialize<CodeBase>(value);
           if (code == null)  throw new Exception("无法处理的未知消息类型");
           MessageHandleFactory.HandleMessage(session, code);
        }
        
        void appServer_NewSessionConnected(WebSocketSession session)
        {
            if (session.Cookies == null || !session.Cookies.ContainsKey("SN") || !session.Cookies.ContainsKey("MAC"))
            {
                //根据分配的远程听诊ID，确认为一次回话连接
                if (session.Cookies.ContainsKey("RemoteID"))
                {
                    if (!Common.RemoteSession.ContainsKey(session.SessionID))
                    Common.RemoteSession.Add(session.SessionID, session.Cookies["RemoteID"]);
                    var remoteID = session.Cookies["RemoteID"];
                    var code = new RYHDLCode();
                    var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
                    //线程安全考虑 不遍历字典 而是遍历字典的key
                    var keys = Common.RemoteSession.Keys.Where(d => d != session.SessionID).ToArray();
                    if (keys == null || keys.Length==0)
                    {
                        session.Send("等待小伙伴加入...");
                        return;
                    }
                    foreach (var key in keys)
                    {
                        var sess = session.AppServer.GetSessionByID(key);
                        //告诉别人自己上线了
                        sess.Send(bytes, 0, bytes.Length);
                        //告诉自己别人上线了
                        session.Send(bytes, 0, bytes.Length);
                    }
                    return;
                }

                if (session.Cookies != null)
                    foreach (string cookie in session.Cookies.Keys)
                    {
                        var key = cookie;
                        var value = session.Cookies[key];
                    }
                session.Send("未经授权的账户,会话关闭");
                session.Close(SuperSocket.SocketBase.CloseReason.ProtocolError);
                return;
            }
            if (!string.IsNullOrEmpty(session.Cookies["SN"]))
            {
                var registCode = SecurityHelper.GetRegistCode(session.Cookies["SN"], session.Cookies["MAC"]);
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(registCode);
                session.Send(bytes, 0, bytes.Length);
                if (!registCode.isLegal)
                {
                    session.Send("软件过期，请联系软件服务商！！！");
                   // session.Close();
                }
                if (!MessageHandleFactory.Dict_Session_Token.ContainsKey(session.Cookies["MAC"]))
                {
                    MessageHandleFactory.Dict_Session_Token.Add(session.Cookies["MAC"], session.SessionID);
                    if (!Common.OnlineUser.ContainsKey(session.Cookies["MAC"]))
                    Common.OnlineUser.Add(session.Cookies["MAC"], session.Cookies["SN"]);
                }
            }

            Invoke(new MethodInvoker(() =>
                {
                    session.Send("连接服务器成功");
                     
                    string sql = string.Format(@"insert into UserLoginLog (EndPoint,SN,MAC,SessionID,LogType,OccTime)
  select '{0}','{1}','{2}','{3}','{4}','{5}'", session.RemoteEndPoint.ToString(), session.Cookies["SN"], session.Cookies["MAC"]
                                            ,session.SessionID,"登录成功",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") );
                    SqlHelper.ExecuteNonQuery(Setting.connectionString, CommandType.Text, sql);
                }));


        }
        void appServer_NewMessageReceived(WebSocketSession session, string value)
        {
            if (session.Cookies == null)
            {
                session.Send("未经授权的账户,会话关闭");
                session.Close(SuperSocket.SocketBase.CloseReason.ProtocolError);
                return;
            }
            Invoke(new MethodInvoker(()=>
                {
            //session.Send("服务端收到了客户端发来的消息");
            //this.listBox1.Items.Add(value);
                }));
             
        }
        void appServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            if (session.Cookies != null && session.Cookies.ContainsKey("RemoteID"))
            {
                if (Common.RemoteSession.ContainsKey(session.SessionID))
                    Common.RemoteSession.Remove(session.SessionID);

                var remoteID = session.Cookies["RemoteID"];
                var yhxxcode = new RYHXXCode();
                var _bytes = ProtocalData.Utilities.SerializaHelper.Serialize(yhxxcode);
                //线程安全考虑 不遍历字典 而是遍历字典的key
                var keys = Common.RemoteSession.Keys.Where(d => d != session.SessionID).ToArray();
                foreach (var key in keys)
                {
                    var sess = session.AppServer.GetSessionByID(key);
                    sess.Send(_bytes, 0, _bytes.Length);
                }
                return;
            }
           
            if (session.Cookies != null && session.Cookies.ContainsKey("MAC"))
            {
                if (MessageHandleFactory.Dict_Session_Token.ContainsKey(session.Cookies["MAC"]))
                {
                    MessageHandleFactory.Dict_Session_Token.Remove(session.Cookies["MAC"]);
                }
                if (Common.OnlineUser.ContainsKey(session.Cookies["MAC"]))
                    Common.OnlineUser.Remove(session.Cookies["MAC"]);

                string sql = string.Format(@"insert into UserLoginLog (EndPoint,SN,MAC,SessionID,LogType,OccTime)
  select '{0}','{1}','{2}','{3}','{4}','{5}'", session.RemoteEndPoint.ToString(), session.Cookies["SN"], session.Cookies["MAC"]
                                           , session.SessionID, "退出成功", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SqlHelper.ExecuteNonQuery(Setting.connectionString, CommandType.Text, sql);
            }
            Invoke(new MethodInvoker(() =>
                {

                    //this.listBox1.Items.Add(value.ToString());
                }));
            var code = new OffLineCode()
            {
                OffLineMac = session.Cookies["MAC"]
            };
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
            foreach (var sess in session.AppServer.GetAllSessions())
            {
                sess.Send(bytes, 0, bytes.Length);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Common.loggerHelper.SaveMessage(new Logger.LogMessage()
                {
                   SN="eded",
                   MAC="dd",
                   EndPoint="edde",
                   CodeName="ffff",
                   ExceptionString="dddddd"
                }, 1);
        }
    }
}

using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;

public class ProtobufClient : MonoBehaviour
{
    private Socket clientSocket;
    private Thread receiveThread; // 子线程接收数据，避免阻塞 Unity 主线程

    void Start()
    {
        ConnectToServer();
    }

    // 连接服务器
    void ConnectToServer()
    {
        clientSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
        );
        try
        {
            clientSocket.Connect("127.0.0.1", 8888); // 连接本地服务器
            Debug.Log("连接服务器成功");

            // 启动子线程接收数据
            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        catch (Exception ex)
        {
            Debug.LogError("连接失败：" + ex.Message);
        }
    }

   
    // 子线程接收服务器数据
    void ReceiveData()
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            try
            {
                int receiveLength = clientSocket.Receive(buffer);
                if (receiveLength <= 0) break;

                // // 解析 Protobuf 消息
                // NetworkMessage message = NetworkMessage.Parser.ParseFrom(buffer, 0, receiveLength);
                // Debug.Log($"收到服务器消息：{message.Type}");
                //
                // // 处理响应（示例：更新UI或游戏对象位置）
                // if (message.Type == NetworkMessage.Types.Type.Position)
                // {
                //     var pos = message.Position;
                //     Debug.Log($"服务器返回位置：({pos.X}, {pos.Y}, {pos.Z})");
                // }
            }
            catch (Exception ex)
            {
                Debug.LogError("接收错误：" + ex.Message);
                break;
            }
        }
    }

    void OnDestroy()
    {
        // 关闭连接和线程
        if (clientSocket != null) clientSocket.Close();
        if (receiveThread != null && receiveThread.IsAlive) receiveThread.Abort();
    }
}
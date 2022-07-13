using System;
class Program
{
    public static void Main()
    {
        SocketClient();
    }
    public static void SocketClient()
    {

        //サーバーのIPアドレスとポート番号を設定
        string ipOrHost = "127.0.0.1";
        int port = 2000;

        //TCPネットワークサービス用のクライアント接続を提供する
        System.Net.Sockets.TcpClient TCP = new System.Net.Sockets.TcpClient(ipOrHost, port);
        Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
                    ((System.Net.IPEndPoint)TCP.Client.RemoteEndPoint).Address,
                    ((System.Net.IPEndPoint)TCP.Client.RemoteEndPoint).Port,
                    ((System.Net.IPEndPoint)TCP.Client.LocalEndPoint).Address,
                    ((System.Net.IPEndPoint)TCP.Client.LocalEndPoint).Port);
        //ネットワーク上のデータのやり取りを可能にする
        System.Net.Sockets.NetworkStream ns = TCP.GetStream();

        bool run = true;
        while (run)
        {
            //サーバーに送るデータを入力
            Console.WriteLine("文字列を入力し、Enterキーを入力してください。");
            string sendMsg = Console.ReadLine();
            if (sendMsg == null || sendMsg.Length == 0)
            {
                run = false;
            }
        }

        Console.ReadLine();
        TCP.Close();
    }
}
using System;
class Program
{
    public static void Main()
    {
        SocketServer();
    }
    public static void SocketServer()
    {
        //ListenするIPアドレスを設定
        //ipアドレスの文字列をIPAddressにインスタンスに変換する
        string ipString = "127.0.0.1";
        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ipString);

        //Listenするポート番号を設定
        int port = 2000;

        //TCP ネットワーク クライアントからの接続をListenする
        System.Net.Sockets.TcpListener Server = new System.Net.Sockets.TcpListener(ipAdd, port);

        //Listenを開始する(イメージ：電話をかけている状態)
        Server.Start();

        //Server側のIPアドレスとポート番号を取得するためにLocalEndpointを使用する
        //その際にSystem.Net.IPEndPointにキャストする必要がある
        Console.WriteLine("Listenを開始しました({0}:{1})。",
            ((System.Net.IPEndPoint)Server.LocalEndpoint).Address,
            ((System.Net.IPEndPoint)Server.LocalEndpoint).Port);

        //(AcceptTcpClient)保留中の接続要求があったら受け入れる(イメージ：受話器を上げる状態)
        //複数の接続が可能
        System.Net.Sockets.TcpClient Client = Server.AcceptTcpClient();
        
        Console.WriteLine("クライアント({0}:{1})と接続しました。",
        ((System.Net.IPEndPoint)Client.Client.RemoteEndPoint).Address,
        ((System.Net.IPEndPoint)Client.Client.RemoteEndPoint).Port);

        Console.WriteLine("Listenerを閉じます。");
        Server.Stop();
        Console.WriteLine("Listenerを閉じました。");
        
        Console.ReadLine();
        //意図的に終了させる
        Client.Close();

    }
}




using System;
using System.IO;

class Program
{
    public static void Main(string[] args)
    {

        string ipOrHost;

        if (args.Length == 0)
        {
            ipOrHost = "127.0.0.1";
        }
        else
        {
            ipOrHost = args[0];
        }

        //サーバーのIPアドレスとポート番号を設定

        int port = 2000;

        //TCPネットワークサービス用のクライアント接続を提供する
        System.Net.Sockets.TcpClient TCP = new System.Net.Sockets.TcpClient(ipOrHost, port);
        Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
                    ((System.Net.IPEndPoint)TCP.Client.RemoteEndPoint).Address,
                    ((System.Net.IPEndPoint)TCP.Client.RemoteEndPoint).Port,
                    ((System.Net.IPEndPoint)TCP.Client.LocalEndPoint).Address,
                    ((System.Net.IPEndPoint)TCP.Client.LocalEndPoint).Port);

        //ネットワーク上のデータの送受信にGetSteamを使用
        System.Net.Sockets.NetworkStream ns = TCP.GetStream();

        //サーバーに送るデータを入力
        //何も入力されていない時・リターンキーのみ・end入力された時はループを抜ける
        int c;
        bool run = true;
        string str = "end";
        while (run)
        {
            Console.WriteLine("文字を入力し、Enterキーで送信してください。");
            string sendMsg = Console.ReadLine();
            c = string.Compare(sendMsg.Trim(), str, true);
            if (sendMsg == null || sendMsg.Length == 0 || c == 0)
            {
                run = false;
            }
            //string型からbyte配列型に変更する(エンコードの指定も必要になるためUTF8に指定)
            //string型からbyte配列型に変更する時にGetBytesを使用する
            //文字列(string型)はUnicodeでは送信することができないのでbyte型として変更する
            System.Text.Encoding nc = System.Text.Encoding.UTF8;
            byte[] sendbytes = nc.GetBytes(sendMsg + '\n');

            //データを送信
            ns.Write(sendbytes, 0, sendbytes.Length);
            //Console.WriteLine(sendMsg);
        }


        //Console.ReadLine();
        TCP.Close();
    }
}
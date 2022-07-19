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

        bool inputrun = true;
        bool run = true;

        int c;
        int Size = 0;

        string str = "end";

        byte[] resbytes = new byte[256];

        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        while (run)
        {
            Console.WriteLine("[文字を入力し、Enterキーで送信してください]");
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

            //Client側のデータ受信
            if (c != 0)
            {
                while (inputrun)
                {
                    //送信されたデータを読み取り、バイト配列に格納する
                    //NetworkStream.Read(格納するByte配列,データを格納を開始する場所，読み取るバイト数)
                    Size = ns.Read(resbytes, 0, resbytes.Length);

                    //読み取るデータが無くなった時・改行のみ入力された時・改行が来た時
                    //データを読み取る処理を終了する

                    if (Size == 0 || resbytes[Size - 1] == '\n')
                    {
                        inputrun = false;
                    }

                    //読み取ったデータをresbytes(byte型)に格納する
                    ms.Write(resbytes, 0, Size);
                }
                //読み取ったbyte型をstring型に変換する
                string resmsg = nc.GetString(resbytes, 0, Size);

                //end入力がされた時の判定
                string aftertrim = resmsg.Trim();
                c = string.Compare(aftertrim, str, true);
                if (c == 0)
                {
                    Console.WriteLine("{0}と入力されました。", aftertrim);
                    run = false;
                }
                else
                {
                    Console.WriteLine("[受信した内容を表示します]");
                    Console.WriteLine(aftertrim);
                    inputrun = true;
                }
            }
        }

        Console.WriteLine("Clientを閉じます。リターンキーを入力してください。");
        Console.ReadLine();
        Console.WriteLine("Clientを閉じました。");
        TCP.Close();
    }
}
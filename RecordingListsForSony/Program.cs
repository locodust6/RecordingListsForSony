using System.Text;
using System.Xml.Serialization;
using OpenSource.UPnP;
using RecordingListsForSony;

public class Program
{
    public static void Main(string[] args)
    {
        //BDZ-EW1200では動作確認済み

        // UPnPデバイスを検索
        UPnPDevice? device = DiscoverUPnPDevices().Where(d => d.ModelName == "Sony-BDZ").FirstOrDefault();
        if (device == null)
        {
            Console.WriteLine("Device not found.");
            return;
        }

        Console.WriteLine("Found device: " + device.FriendlyName);
        // 録画一覧を取得
        GetRecordingList(device);
    }

    private static List<UPnPDevice> DiscoverUPnPDevices()
    {
        List<UPnPDevice> devices = new List<UPnPDevice>();
        UPnPSmartControlPoint scp = new UPnPSmartControlPoint(OnDeviceAdded);
        
        // デバイスが追加されたときのコールバック
        void OnDeviceAdded(UPnPSmartControlPoint sender, UPnPDevice device)
        {
            if (device.DeviceURN == "urn:schemas-upnp-org:device:MediaServer:1")
            {
                devices.Add(device);
            }
        }

        // 一定時間待機してデバイスを収集
        System.Threading.Thread.Sleep(5000); // 5秒待機

        return devices;
    }

    private static void GetRecordingList(UPnPDevice device)
    {
        //録画サービスを取得
        var recordService = device.GetServices("urn:schemas-xsrs-org:service:X_ScheduledRecording:2").FirstOrDefault();
        if (recordService == null)
        {
            Console.WriteLine("Recording Service not found.");
            return;
        }

        // 録画一覧を取得するためのリクエストを送信
        RecordTitleList allRecordTitleList = new();
        for (uint i = 0; i < 1000; i+=200)
        {
            var action = recordService.GetAction("X_GetTitleList");
            action.GetArg("SearchCriteria").DataType = "string";
            action.GetArg("SearchCriteria").DataValue = "";
            action.GetArg("Filter").DataType = "string";
            action.GetArg("Filter").DataValue = "*";
            action.GetArg("StartingIndex").DataType = "ui4";
            action.GetArg("StartingIndex").DataValue = i;
            action.GetArg("RequestedCount").DataType = "ui4";
            action.GetArg("RequestedCount").DataValue = (uint)200;
            action.GetArg("SortCriteria").DataType = "string";
            action.GetArg("SortCriteria").DataValue = "-scheduledStartDateTime";
            recordService.InvokeSync("X_GetTitleList", action.ArgumentList);

            // レスポンスを解析して録画一覧を表示
            string result = action.GetArg("Result").DataValue.ToString();
            if (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Action Fail.");
                return;
            }
            Console.WriteLine($"Got title {i} to {i+200}.");

            RecordTitleList recordTitleList = (DeserializeXml<RecordTitleList>(result));
            allRecordTitleList.Items.AddRange(recordTitleList.Items);
        }
        // CSV出力
        string csvData = ConvertToCsv(allRecordTitleList);
        File.WriteAllText($"{DateTime.Now:yyyyMMdd_HHmmss_}output.csv", csvData);

        Console.WriteLine("CSV file has been created successfully.");
    }


    private static T DeserializeXml<T>(string xml)
    {
        XmlSerializer serializer = new(typeof(T));
        using StringReader reader = new(xml);
        return (T)serializer.Deserialize(reader);
    }

    private static string ConvertToCsv(RecordTitleList recordTitleList)
    {
        StringBuilder csvBuilder = new();
        csvBuilder.AppendLine("Title,StartDateTime,EndDateTime,DestinationID");

        foreach (var item in recordTitleList.Items)
        {
            csvBuilder.AppendLine($"{item.Title},{item.ScheduledStartDateTime.Value},{item.ScheduledStartDateTime.Value.AddSeconds(item.ScheduledDuration)},{item.RecordDestinationID}");
        }

        return csvBuilder.ToString();
    }
}
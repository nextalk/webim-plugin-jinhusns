using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Json;

namespace Spacebuilder.Webim
{
    /*
     * Jsonable object interface
     */
    public abstract class WebimObject
    {

        public Dictionary<string, object> Data()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            feed(data);
            return data;
        }

        public JsonObject Json()
        {
            Dictionary<string, JsonValue> d = new Dictionary<string, JsonValue>();
            var e = Data().GetEnumerator();
            while (e.MoveNext())
            {
                var pair = e.Current;
                d[pair.Key] = (JsonValue)pair.Value;
            }
            return new JsonObject(d);
        }

        public abstract Dictionary<string, object> feed(Dictionary<string, object> data);
        //JsonObject toJson();
    }

    /*
     * Webim User
     * vid:001
     * uid:19919
     */
    public class WebimEndpoint : WebimObject
    {

        /*
         * URI Examples:
         * 
         * vid:001
         * uid:19398
         * sid:echo
         */
        public WebimEndpoint(string id, string uri, string nick)
        {
            Id = id;
            Uri = uri;
            Nick = nick;
            Presence = "offline";
            Show = "unavailable";
            Status = "";
            StatusTime = "";
            Url = "";
            PicUrl = "";
        }

        public override Dictionary<string, object> feed(Dictionary<string, object> data)
        {
            data["id"] = Id;
            data["uri"] = Uri;
            data["nick"] = Nick;
            data["presence"] = Presence;
            data["show"] = Show;
            data["status"] = Status;
            data["status_time"] = StatusTime;
            data["url"] = Url;
            data["pic_url"] = PicUrl;
            return data;
        }

        public string Id { get; set; }

        public string Uri { get; set; }

        public string Nick { get; set; }

        public string Presence { get; set; }

        public string Show { get; set; }

        public string Status { get; set; }

        public string StatusTime { get; set; }

        public string Url { get; set; }

        public string PicUrl { get; set; }

    }

    public class WebimGroup : WebimObject
    {
        public WebimGroup(string id, string uri, string nick)
        {
            Id = id;
            Uri = uri;
            Nick = nick;
            Count = 0;
            AllCount = 0;
            Url = "";
            PicUrl = "";
        }

        public override Dictionary<string, object> feed(Dictionary<string, object> data)
        {
            data["id"] = Id;
            data["uri"] = Uri;
            data["nick"] = Nick;
            data["url"] = Url;
            data["pic_url"] = PicUrl;
            data["count"] = Count;
            data["all_count"] = AllCount;
            return data;
        }

        public string Id { get; set; }

        public string Uri { get; set; }

        public string Nick { get; set; }

        public int Count { get; set; }

        public int AllCount { get; set; }

        public string Url { get; set; }

        public string PicUrl { get; set; }
    }

    /*
     * Webim Message
     */
    public class WebimMessage : WebimObject
    {

        public WebimMessage(string type, string to, string nick, string body, string style, double timestamp)
        {
            Type = type;
            To = to;
            Nick = nick;
            Body = body;
            Style = style;
            Timestamp = timestamp;
        }

        public override Dictionary<string, object> feed(Dictionary<string, object> data)
        {
            data.Add("type", Type);
            data.Add("to", To);
            data.Add("nick", Nick);
            data.Add("body", Body);
            data.Add("style", Style);
            data.Add("timestamp", Timestamp);
            return data;
        }


        public string Type { get; set; }

        public string Nick { get; set; }

        public string To { get; set; }

        public string Body { get; set; }

        public string Style { get; set; }

        public double Timestamp { get; set; }

    }

    public class WebimPresence : WebimObject
    {

        public WebimPresence(string show, string status)
        {
            Show = show;
            Status = status;
        }

        public override Dictionary<string, object> feed(Dictionary<string, object> data)
        {
            data.Add("show", Show);
            data.Add("status", Status);
            return data;
        }

        public string Show { get; set; }

        public string Status { get; set; }

    }

    public class WebimStatus : WebimObject
    {
        public WebimStatus(string to, string show, string status)
        {
            To = to;
            Show = show;
            Status = status;
        }

        public override Dictionary<string, object> feed(Dictionary<string, object> data)
        {
            data.Add("to", To);
            data.Add("show", Show);
            data.Add("status", Status);
            return data;
        }

        public string To { get; set; }
        public string Show { get; set; }
        public string Status { get; set; }
    }

    public class WebimHistory : WebimObject
    {

        public WebimHistory()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public override Dictionary<string, object> feed(Dictionary<string, object> data)
        {
            data["id"] = Id;
            data["type"] = Type;
            data["send"] = Send == 1 ? "true" : "false";
            data["to"] = To;
            data["from"] = From;
            data["nick"] = Nick;
            data["body"] = Body;
            data["style"] = Style;
            data["timestamp"] = Timestamp;
            return data;
        }

        public long Id { get; set; }

        public int Send { get; set; }

        public string Type { get; set; }

        public string To { get; set; }

        public string From { get; set; }

        public string Nick { get; set; }

        public string Body { get; set; }

        public string Style { get; set; }

        public double Timestamp { get; set; }

        public int ToDel { get; set; }

        public int FromDel { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }

    public class WebimException : System.Exception
    {
        private int code;

        public WebimException(int code, string status)
            : base(status)
        {
            this.code = code;
        }

        public int getCode()
        {
            return code;
        }

    }

    public class WebimClient
    {
        private int port;
        private WebimEndpoint ep;
        private string domain;
        private string apikey;
        private string host;
        private string ticket = "";

        public WebimClient(WebimEndpoint ep, string domain, string apikey, string host, int port)
        {
            this.ep = ep;
            this.host = host;
            this.port = port;
            this.domain = domain;
            this.apikey = apikey;
        }

        public WebimEndpoint Endpoint
        {
            get { return ep; }
            set { ep = value; }
        }

        public string Ticket
        {
            get { return ticket; }
            set { ticket = value; }
        }

        public string Domain
        {
            get { return domain; }
        }

        /**
         * User Online
         * 
         * @return JsonObject
         */
        public JsonObject Online(IEnumerable<string> buddies, IEnumerable<string> groups)
        {
            Dictionary<string, object> data = NewData();
            data.Add("groups", this.ListJoin(",", groups));
            data.Add("buddies", this.ListJoin(",", buddies));
            data.Add("uri", ep.Uri);
            data.Add("id", ep.Id);
            data.Add("name", ep.Id);
            data.Add("nick", ep.Nick);
            data.Add("status", ep.Status);
            data.Add("show", ep.Show);
            JsonObject json = HttpPost("/presences/online", data);
            this.ticket = (string)json["ticket"];
            return json;
        }


        /**
        * User Offline
        *
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws Exception
        */
        public JsonObject Offline()
        {
            Dictionary<string, object> data = NewData();
            return HttpPost("/presences/offline", data);
        }

        /**
        * Publish updated presence.
        *
        * @param presence
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws Exception
        */
        public JsonObject Publish(WebimPresence presence)
        {
            Dictionary<string, object> data = NewData();
            data.Add("nick", ep.Nick);
            presence.feed(data);
            return HttpPost("/presences/show", data);
        }

        /**
        * Publish status
        * @param status
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws Exception
        */
        public JsonObject Publish(WebimStatus status)
        {
            Dictionary<string, object> data = NewData();
            data.Add("nick", ep.Nick);
            status.feed(data);
            return HttpPost("/statuses", data);
        }

        /**
        * Publish Message
        * @param message
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws Exception
        */
        public JsonObject Publish(WebimMessage message)
        {
            Dictionary<string, object> data = NewData();
            message.feed(data);
            return HttpPost("/messages", data);
        }

        /**
        * Push Message, no need to be online
        * @param from
        * @param message
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws Exception
        */
        public JsonObject Publish(String from, WebimMessage message)
        {
            Dictionary<string, object> data = NewData();
            data.Add("from", from);
            message.feed(data);
            return HttpPost("/messages", data);
        }

        /**
        * Get presences
        * @param grpid
        * @return member list
        * @throws Exception
        */
        public JsonObject Presences(IEnumerable<string> ids)
        {
            Dictionary<string, object> data = NewData();
            data.Add("ids", this.ListJoin(",", ids));
            return (JsonObject)HttpGet("/presences", data);
        }

        /**
        * Get group members
        * @param grpid
        * @return member list
        * @throws Exception
        */
        public JsonArray Members(string grpid)
        {
            Dictionary<string, object> data = NewData();
            data.Add("group", grpid);
            return (JsonArray)HttpGet("/group/members", data);
        }

        /**
        * Join Group
        * @param grpid
        * @return JsonObject "{'id': 'grpid', 'count': '0'}"
        * @throws Exception
        */
        public JsonObject Join(string grpid)
        {
            Dictionary<string, object> data = NewData();
            data.Add("nick", ep.Nick);
            data.Add("group", grpid);
            return HttpPost("/group/join", data);
        }

        /**
        * Leave Group
        * @param grpid
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws Exception
        */
        public JsonObject Leave(string grpid)
        {
            Dictionary<string, object> data = NewData();
            data.Add("nick", ep.Nick);
            data.Add("group", grpid);
            return HttpPost("/group/leave", data);
        }

        private JsonValue HttpGet(string path, Dictionary<string, object> parameters)
        {
            String url = this.ApiUrl(path);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + "?" + UrlEncode(parameters));
            using (var response = req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    string strRecv = sr.ReadToEnd();
                    return JsonObject.Parse(strRecv);

                }
            }
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = client.GetAsync(url + "?" + UrlEncode(parameters)).Result;
            //response.EnsureSuccessStatusCode();
            //string content = response.Content.ReadAsStringAsync().Result;
            //return (JsonObject)JsonObject.Parse(content);
        }

        private string UrlEncode(Dictionary<string, object> parameters)
        {
            List<string> l = new List<string>();
            foreach (KeyValuePair<string, object> p in parameters)
            {
                //TODO: FIXME Later
                l.Add(p.Key + "=" + Uri.EscapeUriString(p.Value != null ? p.Value.ToString() : string.Empty));
            }
            return string.Join("&", l.ToArray());
        }

        private JsonObject HttpPost(string path, Dictionary<string, object> data)
        {
            //String url = this.ApiUrl(path);
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(data.AsEnumerable())).Result;
            //response.EnsureSuccessStatusCode();
            //string content = response.Content.ReadAsStringAsync().Result;
            //return (JsonObject)JsonObject.Parse(content);

            String url = this.ApiUrl(path);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            Encoding encoding = Encoding.UTF8;
            byte[] bs = Encoding.UTF8.GetBytes(UrlEncode(data));
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
            }
            using (var response = req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                {
                    string strRecv = sr.ReadToEnd();
                    return (JsonObject)JsonObject.Parse(strRecv);
                }
            }
        }

        private Dictionary<string, object> NewData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("version", "v5");
            data.Add("domain", Domain);
            data.Add("apikey", apikey);
            if (!Ticket.Equals(""))
            {
                data.Add("ticket", Ticket);
            }
            return data;
        }

        private string ListJoin(string sep, IEnumerable<string> list)
        {
            bool first = true;
            StringBuilder sb = new StringBuilder();
            foreach (string g in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sep);
                }
                sb.Append(g);

            }
            return sb.ToString();
        }

        private string ApiUrl(string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            return "http://" + host + ":" + port.ToString() + "/v5" + path;
        }

    }

}


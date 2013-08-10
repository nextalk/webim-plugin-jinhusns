using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Json;

namespace Webim
{
    /*
     * Jsonable object interface
     */
    public abstract class WebimObject
    {

        public Dictionary<string, string> Data()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
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

        abstract public void feed(Dictionary<string, string> data);
        //JsonObject toJson();
    }

    /*
     * Webim User
     * vid:001
     * uid:19919
     */
    public class WebimEndpoint : WebimObject
    {
        private string id;
        /*
         * URI Examples:
         * 
         * vid:001
         * uid:19398
         * sid:echo
         */
        private string uri;
        private string nick;
        private string show;
        private string status;
        //last time
        private string status_time;
        //space url
        private string url;
        //avatar url
        private string pic_url;

        public WebimEndpoint(string id, string uri, string nick)
        {
            this.id = id;
            this.uri = uri;
            this.nick = nick;
            this.show = "available";
            this.status = "Online";
            this.status_time = "";
            this.url = "";
            this.pic_url = "";
        }

        public string Id
        {
            get { return id; }
        }

        public string Uri
        {
            get { return uri; }
        }

        public string Nick
        {
            get { return nick; }
            set { this.nick = value; }
        }

        public string Show
        {
            get { return show; }
            set { this.show = value; }
        }

        public string Status
        {
            get { return status; }
            set { this.status = value; }
        }

        public string StatusTime
        {
            get { return status_time; }
            set { this.status_time = value; }
        }

        public string Url
        {
            get { return url; }
            set { this.url = value; }
        }

        public string PicUrl
        {
            get { return pic_url; }
            set { this.pic_url = value; }
        }

        public override void feed(Dictionary<string, string> data)
        {
            data["id"] = id;
            data["uri"] = uri;
            data["nick"] = nick;
            data["show"] = show;
            data["status"] = status;
            data["status_time"] = status_time;
            data["url"] = url;
            data["pic_url"] = pic_url;
        }

    }

	public class WebimGroup : WebimObject
	{
		private string id;
		private string uri;
		private string nick;
		private int count;
		
		public WebimGroup(string id, string uri, string nick)
		{
			this.id = id;
			this.uri = uri;
			this.nick = nick;
			this.count = 0;
		}

		public string Id
		{
			get { return id; }
			set { this.id = value; }
		}

		public string Uri
		{
			get { return uri; }
			set { this.uri = value; }
		}

		public string Nick
		{
			get { return nick; }
			set { this.nick = value; }
		}

		public int Count
		{
			get { return count; }
			set { this.count = value; }
		}

		public override void feed(Dictionary<string, string> data)
		{
			data["id"] = id;
			data["uri"] = uri;
			data["nick"] = nick;
			data["count"] = count.toString();
		}

	}

    /*
     * Webim Message
     */
    public class WebimMessage : WebimObject
    {
        private string to;
        private string body;
        private string style;
        private int timestamp;

        public WebimMessage(string to, string body, string style, int timestamp)
        {
            this.to = to;
            this.body = body;
            this.style = style;
            this.timestamp = timestamp;
        }

        public string To
        {
            get { return to; }
            set { this.to = value; }
        }

        public string Body
        {
            get { return body; }
            set { this.body = value; }
        }

        public string Style
        {
            get { return style; }
            set { this.style = value; }
        }

        public int Timestamp
        {
            get { return timestamp; }
            set { this.timestamp = value; }
        }

        public override void feed(Dictionary<string, string> data)
        {
            data.Add("to", to);
            data.Add("body", body);
            data.Add("style", style);
            data.Add("timestamp", timestamp.ToString());
        }

    }

    public class WebimPresence : WebimObject
    {

        private string show;

        private string status;

        public WebimPresence(string show, string status)
        {
            this.show = show;
            this.status = status;
        }

        public override void feed(Dictionary<string, string> data)
        {
            data.Add("show", show);
            data.Add("status", status);
        }

    }

    public class WebimStatus : WebimObject
    {

        private string to;

        private string show;

        public WebimStatus(string to, string show, string status)
        {
            this.to = to;
            this.show = show;
        }

        public override void feed(Dictionary<string, string> data)
        {
            data.Add("to", to);
            data.Add("show", show);
        }
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

        public JsonObject Online(List<string> buddies, List<string> groups)
        {

            Dictionary<string, string> data = NewData();
            data.Add("groups", this.ListJoin(",", groups));
            data.Add("buddies", this.ListJoin(",", buddies));
            data.Add("uri", ep.Uri);
            data.Add("id", ep.Id);
            data.Add("name", ep.Id);
            data.Add("nick", ep.Nick);
            data.Add("status", ep.Status);
            data.Add("show", ep.Show);
            try
            {
                JsonObject respObj = HttpPost("/presences/online", data);
                this.ticket = (string)respObj["ticket"];

                JsonObject connInfo = new JsonObject();
                connInfo.Add("ticket", Ticket);
                connInfo.Add("domain", Domain);
                string jsonpd = (string)respObj["jsonpd"];
                connInfo.Add("server", jsonpd);
                connInfo.Add("jsond", jsonpd);
				connInfo.Add("websocket", (string)respObj["websocket"]),

                JsonObject rtObj = new JsonObject();
                rtObj.Add("success", true);
                rtObj.Add("conn", connInfo);
                rtObj.Add("buddies", respObj["buddies"]);
                rtObj.Add("groups", respObj["groups"]);
                rtObj.Add("server_time", 1000);// FIXME LATER
                rtObj.Add("user", ep.Json());
                return rtObj;
            }
            catch (System.Exception e)
            {
                throw new WebimException(500, e.Message);
            }
        }


        /**
        * User Offline
        *
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws WebIMException
        */
        public JsonObject Offline()
        {
            Dictionary<string, string> data = NewData();
            try
            {
                return HttpPost("/presences/offline", data);
            }
            catch (Exception e)
            {
                throw new WebimException(500, e.Message);
            }
        }

        /**
        * Publish updated presence.
        *
        * @param presence
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws WebIMException
        */
        public JsonObject Publish(WebimPresence presence)
        {
            Dictionary<string, string> data = NewData();
            data.Add("nick", ep.Nick);
            presence.feed(data);
            try
            {
                return HttpPost("/presences/show", data);
            }
            catch (Exception e)
            {
                throw new WebimException(500, e.Message);
            }
        }

        /**
        * Publish status
        * @param status
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws WebIMException
        */
        public JsonObject Publish(WebimStatus status)
        {
            Dictionary<string, string> data = NewData();
            data.Add("nick", ep.Nick);
            status.feed(data);
            try
            {
                return HttpPost("/statuses", data);
            }
            catch (Exception e)
            {
                throw new WebimException(500, e.Message);
            }
        }

        /**
        * Publish Message
        * @param message
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws WebIMException
        */
        public JsonObject Publish(WebimMessage message)
        {
            Dictionary<string, string> data = NewData();
            data.Add("type", "unicast"); //TODO: FIXLATER
            data.Add("nick", ep.Nick);
            message.feed(data);
            try
            {
                return HttpPost("/messages", data);
            }
            catch (Exception e)
            {
                throw new WebimException(500, e.Message);
            }
        }

        /**
        * Get group members
        * @param grpid
        * @return member list
        * @throws WebIMException
        */
        public JsonObject Members(string grpid)
        {
            Dictionary<string, string> data = NewData();
            data.Add("group", grpid);
            try
            {
                return HttpGet("/group/members", data);
            }
            catch (Exception e)
            {
                throw new WebimException(500, e.Message);
            }
        }

        /**
        * Join Group
        * @param grpid
        * @return JsonObject "{'id': 'grpid', 'count': '0'}"
        * @throws WebIMException
        */
        public JsonObject Join(string grpid)
        {
            Dictionary<string, string> data = NewData();
            data.Add("nick", ep.Nick);
            data.Add("group", grpid);
            try
            {
                JsonObject respObj = HttpPost("/group/join", data);
                JsonObject rtObj = new JsonObject();
                rtObj.Add("id", grpid);
                rtObj.Add("count", (int)respObj.GetValue(grpid));
                return rtObj;
            }
            catch (Exception e)
            {
                throw new WebimException(500, e.Message);
            }
        }

        /**
        * Leave Group
        * @param grpid
        * @return JsonObject "{'status': 'ok'}" or "{'status': 'error', 'message': 'blabla'}"
        * @throws WebIMException
        */
        public JsonObject Leave(string grpid)
        {
            Dictionary<string, string> data = NewData();
            data.Add("nick", ep.Nick);
            data.Add("group", grpid);
            try
            {
                return HttpPost("/group/leave", data);
            }
            catch (Exception e)
            {
                throw new WebimException(500, e.Message);
            }
        }

        private JsonObject HttpGet(string path, Dictionary<string, string> parameters)
        {
            String url = this.ApiUrl(path);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url + "?" + UrlEncode(parameters)).Result;
            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;
            return (JsonObject)JsonObject.Parse(content);
        }

        private string UrlEncode(Dictionary<string, string> parameters)
        {
            List<string> l = new List<string>();
            foreach (KeyValuePair<string, string> p in parameters)
            {
                //TODO: FIXME Later
                l.Add(p.Key + "=" + Uri.EscapeUriString(p.Value));
            }
            return string.Join("&", l.ToArray());
        }

        private JsonObject HttpPost(string path, Dictionary<string, string> data)
        {
            String url = this.ApiUrl(path);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(data.AsEnumerable())).Result;
            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;
            return (JsonObject)JsonObject.Parse(content);
        }

        private Dictionary<string, string> NewData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("version", "v4");
            data.Add("domain", Domain);
            data.Add("apikey", apikey);
            data.Add("ticket", Ticket);
            return data;
        }

        private string ListJoin(string sep, List<string> groups)
        {
            bool first = true;
            StringBuilder sb = new StringBuilder();
            foreach (string g in groups)
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
            return "http://" + host + ":" + port.ToString() + "/v4" + path;
        }

    }

}

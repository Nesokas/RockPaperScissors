using System;
using System.Collections.Generic;
using GameSparks.Api.Requests;

namespace GameSparks
{
	public class GameSparksSender
	{
		private IDictionary<string,object> data;

		public GameSparksSender(string requestType){
			this.data = new Dictionary<string, object> ();
			data.Add("@class", "."+requestType);
		}

		public GameSparksSender(IDictionary<string,object> data){
			this.data = data;
		}

		public IDictionary<string,object> send()
		{
			return build().Send().JSONData;
		}

		public void sendAsync(Action<IDictionary<string,object>> callback)
		{
			build ().Send((response) => {callback(response.JSONData);});
		}

		public void sendBackground(Action<IDictionary<string,object>> callback)
		{
			build ().Send((response) => {callback(response.JSONData);});
		}

		public GameSparksSender addParameter(String paramName, object paramValue){
			data.Add(paramName, paramValue);
			return this;
		}

		private IDictionary<String, object> scriptData = new Dictionary<String, object>();

		public GameSparksSender addScriptData(String paramName, object paramValue){
			lock(scriptData){
				if(!data.ContainsKey("scriptData")){
					data.Add("scriptData", scriptData);
				}
			}
			scriptData.Add(paramName, paramValue);
			return this;
		}

		private GameSparks.Api.Requests.CustomRequest build(){
			CustomRequest r = new CustomRequest(data).SetScriptData(new GameSparks.Core.GSRequestData(scriptData));
			return  r;
		}

	}
}


using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_MakeMove : GSTypedRequest<LogEventRequest_MakeMove, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_MakeMove() : base("LogEventRequest"){
			request.AddString("eventKey", "MakeMove");
		}
		public LogEventRequest_MakeMove Set_move( long value )
		{
			request.AddNumber("move", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_MakeMove : GSTypedRequest<LogChallengeEventRequest_MakeMove, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_MakeMove() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "MakeMove");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_MakeMove SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_MakeMove Set_move( long value )
		{
			request.AddNumber("move", value);
			return this;
		}			
	}
	
	public class LogEventRequest_PostScore : GSTypedRequest<LogEventRequest_PostScore, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_PostScore() : base("LogEventRequest"){
			request.AddString("eventKey", "PostScore");
		}
		public LogEventRequest_PostScore Set_score( long value )
		{
			request.AddNumber("score", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_PostScore : GSTypedRequest<LogChallengeEventRequest_PostScore, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_PostScore() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "PostScore");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_PostScore SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_PostScore Set_score( long value )
		{
			request.AddNumber("score", value);
			return this;
		}			
	}
	
}
	
	
	
namespace GameSparks.Api.Requests{
	
	public class LeaderboardDataRequest_HighScoreLeaderboard : GSTypedRequest<LeaderboardDataRequest_HighScoreLeaderboard,LeaderboardDataResponse_HighScoreLeaderboard>
	{
		public LeaderboardDataRequest_HighScoreLeaderboard() : base("LeaderboardDataRequest"){
			request.AddString("leaderboardShortCode", "HighScoreLeaderboard");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LeaderboardDataResponse_HighScoreLeaderboard (response);
		}		
		
		/// <summary>
		/// The challenge instance to get the leaderboard data for
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// The offset into the set of leaderboards returned
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetOffset( long offset )
		{
			request.AddNumber("offset", offset);
			return this;
		}
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public LeaderboardDataRequest_HighScoreLeaderboard SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
		
	}

	public class AroundMeLeaderboardRequest_HighScoreLeaderboard : GSTypedRequest<AroundMeLeaderboardRequest_HighScoreLeaderboard,AroundMeLeaderboardResponse_HighScoreLeaderboard>
	{
		public AroundMeLeaderboardRequest_HighScoreLeaderboard() : base("AroundMeLeaderboardRequest"){
			request.AddString("leaderboardShortCode", "HighScoreLeaderboard");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new AroundMeLeaderboardResponse_HighScoreLeaderboard (response);
		}		
		
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLeaderboard SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLeaderboard SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLeaderboard SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLeaderboard SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLeaderboard SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLeaderboard SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLeaderboard SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
	}
}

namespace GameSparks.Api.Responses{
	
	public class _LeaderboardEntry_HighScoreLeaderboard : LeaderboardDataResponse._LeaderboardData{
		public _LeaderboardEntry_HighScoreLeaderboard(GSData data) : base(data){}
		public long? score{
			get{return response.GetNumber("score");}
		}
	}
	
	public class LeaderboardDataResponse_HighScoreLeaderboard : LeaderboardDataResponse
	{
		public LeaderboardDataResponse_HighScoreLeaderboard(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard> Data_HighScoreLeaderboard{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_HighScoreLeaderboard(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard> First_HighScoreLeaderboard{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_HighScoreLeaderboard(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard> Last_HighScoreLeaderboard{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_HighScoreLeaderboard(data);});}
		}
	}
	
	public class AroundMeLeaderboardResponse_HighScoreLeaderboard : AroundMeLeaderboardResponse
	{
		public AroundMeLeaderboardResponse_HighScoreLeaderboard(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard> Data_HighScoreLeaderboard{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_HighScoreLeaderboard(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard> First_HighScoreLeaderboard{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_HighScoreLeaderboard(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard> Last_HighScoreLeaderboard{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLeaderboard>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_HighScoreLeaderboard(data);});}
		}
	}
}	

namespace GameSparks.Api.Messages {


}

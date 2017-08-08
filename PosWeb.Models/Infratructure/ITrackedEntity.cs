using System;
namespace PosWeb.Models.Infratructure
{
	public interface ITrackedEntity
	{
		DateTime CreatedDate { get; set; }
		string CreatedBy { get; set; }
		DateTime UpdatedDate { get; set; }
		string UpdatedBy { get; set; }
	}
}

using System;
using System.ComponentModel;

namespace CourseStudio.Domain.TraversalModel.Courses
{
	public enum CourseStateEnum
    {
        [Description("Pending")]
        Pending,
        [Description("Auditing")]
        Auditing,
		[Description("Approved")]
        Approved,
		[Description("Rejected")]
		Rejected,
        [Description("Lanched")]
        Lanched
        //[Description("UpdateRequestReview")]
        //UpdateRequestReview // review the update request
    }

    public enum CourseStateTriggerEnum
    {
        [Description("Submit")]
        Submit,
        [Description("Reject")]
        Reject,
        [Description("Approve")]
        Approve,
        [Description("Reopen")]
        Reopen,
        [Description("Release")]
        Release
        //UpdateRequest,
        //UpdateRequestReject,
        //UpdateRequestApprove
    }
}

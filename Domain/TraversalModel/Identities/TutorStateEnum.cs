using System;
using System.ComponentModel;

namespace CourseStudio.Domain.TraversalModel.Identities
{
	public enum TutorStateEnum
    {
        [Description("Pending")]
        Pending,
        [Description("Auditing")]
        Auditing,
		[Description("Approved")]
        Approved,
		[Description("Rejected")]
		Rejected
    }

    public enum TutorStateTriggerEnum
    {
        [Description("Apply")]
        Apply,
        [Description("Reject")]
        Reject,
        [Description("Approve")]
        Approve,
        [Description("Reopen")]
        Reopen
    }
}

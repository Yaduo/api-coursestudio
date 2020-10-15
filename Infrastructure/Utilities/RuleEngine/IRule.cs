using System;
namespace CourseStudio.Lib.Utilities.RuleEngine
{
    public interface IRule
    {
		int Id { get; set; }
		string MemberName { get; set; }
		string Operator { get; set; }
		string TargetValue { get; set; }
		void Update(IRule newRule);
    }
}

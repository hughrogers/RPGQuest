
public class Useable
{
	public bool useInBattle = false;
	public bool useInField = false;
	public TargetType targetType = TargetType.ALLY;
	public SkillTarget skillTarget = SkillTarget.SINGLE;
	public TargetRaycast targetRaycast = new TargetRaycast();
	
	public Useable() {}
	
	// target checks
	public bool TargetSingleAlly()
	{
		return this.TargetAlly() && this.TargetSingle();
	}
	
	public bool TargetAllyGroup()
	{
		return this.TargetAlly() && this.TargetGroup();
	}
	
	public bool TargetSingleEnemy()
	{
		return this.TargetEnemy() && this.TargetSingle();
	}
	
	public bool TargetEnemyGroup()
	{
		return this.TargetEnemy() && this.TargetGroup();
	}
	
	public bool TargetAlly()
	{
		return TargetType.ALLY.Equals(this.targetType);
	}
	
	public bool TargetEnemy()
	{
		return TargetType.ENEMY.Equals(this.targetType);
	}
	
	public bool TargetSelf()
	{
		return TargetType.SELF.Equals(this.targetType);
	}
	
	public bool TargetSingle()
	{
		return SkillTarget.SINGLE.Equals(this.skillTarget);
	}
	
	public bool TargetGroup()
	{
		return SkillTarget.GROUP.Equals(this.skillTarget);
	}
	
	public bool TargetNone()
	{
		return SkillTarget.NONE.Equals(this.skillTarget);
	}
}
namespace HyperCasualRunner
{
	public struct HitDamageInfo
	{
		public HitDamageInfo(int hitDamage, int remainingHitPoint)
		{
			HitDamage = hitDamage;
			RemainingHitPoint = remainingHitPoint;
		}
		public int HitDamage;
		public int RemainingHitPoint;
	}
}

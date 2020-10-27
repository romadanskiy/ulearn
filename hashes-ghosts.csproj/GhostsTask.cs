using System;
using System.Text;

namespace hashes
{
	public class GhostsTask : 
		IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
		IMagic
	{
		private readonly Vector vector = new Vector(20, 01);
		private readonly Segment segment = new Segment(new Vector(19, 39), new Vector(19, 45));
		private readonly static byte[] content = new byte[] {67,111,114,114,117,112,116,105,111,110};
		private readonly Document document = new Document("Constitution", Encoding.UTF8, content);
		private readonly Cat cat = new Cat("Puss", "in Boots", new DateTime(2020, 02, 02));
		private readonly Robot robot = new Robot("R2-D2");

		public void DoMagic()
		{
			vector.Add(new Vector(0, 19));
			segment.Start.Add(new Vector(0, 2));
			content[0]++;
			cat.Rename("Puppy");
			Robot.BatteryCapacity += new Random().Next();
		}

		Vector IFactory<Vector>.Create() => vector;

		Segment IFactory<Segment>.Create() => segment;

		Document IFactory<Document>.Create() => document;

		Cat IFactory<Cat>.Create() => cat;

		Robot IFactory<Robot>.Create() => robot;
	}
}
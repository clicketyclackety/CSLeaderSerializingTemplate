using System;
using System.Collections.Generic;
using System.Linq;
using Rhino;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Commands;


namespace CSLeaderSerializingTemplate
{
	public class CSLeaderSerializingTemplateCommand : Command
	{
		public CSLeaderSerializingTemplateCommand()
		{
			// Rhino only creates one instance of each command class defined in a
			// plug-in, so it is safe to store a refence in a static property.
			Instance = this;
		}

		///<summary>The only instance of this command.</summary>
		public static CSLeaderSerializingTemplateCommand Instance { get; private set; }

		///<returns>The command name as it appears on the Rhino command line.</returns>
		public override string EnglishName => "CSLeaderSerializingTemplateCommand";

		protected override Result RunCommand(RhinoDoc doc, RunMode mode)
		{

			// Create the Leader to test

			DimensionStyle dimstyle = doc.DimStyles.First();

			Point3d[] points = new Point3d[] { new Point3d(0,0,0), new Point3d(0, 10, 0), new Point3d(10, 10, 0) };

			Leader leader = Rhino.Geometry.Leader.Create("Serialize me!", Plane.WorldXY, dimstyle, points);

			doc.Objects.Add(leader);

			// File paths

			string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string filename = Path.Combine(desktop, "mytestfile.bin");

			// Serialize Leader. In my example the leader is in a list. But this works (well, fails) too.

			Stream TestSaveFileStream = File.Create(filename);
			BinaryFormatter saveserializer = new BinaryFormatter();
			saveserializer.Serialize(TestSaveFileStream, leader);
			TestSaveFileStream.Close();

			// DeSerialize the Leader

			Stream TestLoadFileStream = File.OpenRead(filename);
			BinaryFormatter loadserializer = new BinaryFormatter();

			Leader loadleader = (Leader)loadserializer.Deserialize(TestLoadFileStream);

			TestLoadFileStream.Close();

			return Result.Success;
		}
	}
}

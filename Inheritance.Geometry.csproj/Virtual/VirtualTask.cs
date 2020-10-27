using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);

        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= this.Radius * this.Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var size = 2 * this.Radius;
            return new RectangularCuboid(this.Position, size, size, size);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var minPoint = new Vector3(
                Position.X - this.SizeX / 2,
                Position.Y - this.SizeY / 2,
                Position.Z - this.SizeZ / 2);
            var maxPoint = new Vector3(
                Position.X + this.SizeX / 2,
                Position.Y + this.SizeY / 2,
                Position.Z + this.SizeZ / 2);

            return point >= minPoint && point <= maxPoint;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return this;
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - this.SizeZ / 2;
            var maxZ = minZ + this.SizeZ;

            return length2 <= this.Radius * this.Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var sizeXY = 2 * this.Radius;
            return new RectangularCuboid(this.Position, sizeXY, sizeXY, this.SizeZ);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return this.Parts.Any(body => body.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        { 
            var projectionX = new Projection();
            var projectionY = new Projection();
            var projectionZ = new Projection();

            foreach (var body in this.Parts)
            {
                var curCuboid = body.GetBoundingBox();
                var curPosition = curCuboid.Position;
                projectionX.Add(curPosition.X, curCuboid.SizeX);
                projectionY.Add(curPosition.Y, curCuboid.SizeY);
                projectionZ.Add(curPosition.Z, curCuboid.SizeZ);
            }

            return new RectangularCuboid(new Vector3(projectionX.Middle, projectionY.Middle, projectionZ.Middle),
                projectionX.Length, projectionY.Length, projectionZ.Length);
        }
    }

    public class Projection
    {
        private readonly List<double> list;
        
        public double Max { get; private set; }
        public double Min { get; private set; }
        public double Length => Max - Min;
        public double Middle => Min + Length / 2;

        public Projection()
        {
            list = new List<double>();
        }

        public void Add(double position, double size)
        {
            var halfSize = size / 2;
            list.Add(position + halfSize);
            list.Add(position - halfSize);
            Max = list.Max();
            Min = list.Min();
        }
    }
}
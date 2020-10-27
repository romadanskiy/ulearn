using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowPos = GetPointCoordinates(Manipulator.UpperArm, 0, 0, shoulder);
            var wristPos = GetPointCoordinates(Manipulator.Forearm, elbowPos.X, elbowPos.Y, shoulder + elbow + Math.PI);
            var palmEndPos = GetPointCoordinates(Manipulator.Palm, wristPos.X, wristPos.Y, elbow + shoulder + wrist);

            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }

        public static PointF GetPointCoordinates(double arm, double x0, double y0, double angle)
        {
            var x = (float)(x0 + Math.Cos(angle) * arm);
            var y = (float)(y0 + Math.Sin(angle) * arm);

            return new PointF(x, y);
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        public const double PI = Math.PI;
        
        [TestCase(PI / 2, PI / 2, PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(PI / 2, PI, PI, 0, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm)]
        [TestCase(0, PI, PI, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm, 0)]
        [TestCase(PI / 2, PI / 2, PI / 2, Manipulator.Forearm, Manipulator.UpperArm - Manipulator.Palm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            var actualUpperArm = GetDistance(0, 0, joints[0].X, joints[0].Y);
            var actualForearm = GetDistance(joints[0].X, joints[0].Y, joints[1].X, joints[1].Y);
            var actualPalm = GetDistance(joints[1].X, joints[1].Y, joints[2].X, joints[2].Y);
            Assert.AreEqual(Manipulator.UpperArm, actualUpperArm, 1e-5, "upper arm length");
            Assert.AreEqual(Manipulator.Forearm, actualForearm, 1e-5, "forearm length");
            Assert.AreEqual(Manipulator.Palm, actualPalm, 1e-5, "palm length");
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }

        public double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}
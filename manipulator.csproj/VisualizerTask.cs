using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manipulation
{
	public static class VisualizerTask
	{
		public static double X = 220;
		public static double Y = -100;
		public static double Alpha = 0.05;
		public static double Wrist = 2 * Math.PI / 3;
		public static double Elbow = 3 * Math.PI / 4;
		public static double Shoulder = Math.PI / 2;

		public static Brush UnreachableAreaBrush = new SolidBrush(Color.FromArgb(255, 255, 230, 230));
		public static Brush ReachableAreaBrush = new SolidBrush(Color.FromArgb(255, 230, 255, 230));
		public static Pen ManipulatorPen = new Pen(Color.Black, 3);
		public static Brush JointBrush = Brushes.Gray;

		public static void KeyDown(Form form, KeyEventArgs key)
		{
            // TODO: Добавьте реакцию на QAWS и пересчитывать Wrist
            if (key.KeyCode == Keys.Q) { Shoulder += 1; ChangeWrist(); }
            if (key.KeyCode == Keys.A) { Shoulder -= 1; ChangeWrist(); }
            if (key.KeyCode == Keys.W) { Elbow += 1; ChangeWrist(); }
            if (key.KeyCode == Keys.S) { Elbow -= 1; ChangeWrist(); }

            form.Invalidate();
		}

        public static void ChangeWrist()
        {
            Wrist = -Alpha - Shoulder - Elbow;
        }

		public static void MouseMove(Form form, MouseEventArgs e)
		{
            // TODO: Измените X и Y пересчитав координаты (e.X, e.Y) в логические.
            var logicPoint = ConvertWindowToMath(new PointF(e.X, e.Y), GetShoulderPos(form));
            X = logicPoint.X;
            Y = logicPoint.Y;

			UpdateManipulator();
			form.Invalidate();
		}

		public static void MouseWheel(Form form, MouseEventArgs e)
		{
            // TODO: Измените Alpha, используя e.Delta — размер прокрутки колеса мыши
            Alpha += e.Delta;

			UpdateManipulator();
			form.Invalidate();
		}

		public static void UpdateManipulator()
		{
            // Вызовите ManipulatorTask.MoveManipulatorTo и обновите значения полей Shoulder, Elbow и Wrist, 
            // если они не NaN. Это понадобится для последней задачи.
            var angels = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
            if (!double.IsNaN(angels[0]))
            {
                Shoulder = angels[0];
                Elbow = angels[1];
                Wrist = angels[2];
            }
		}

		public static void DrawManipulator(Graphics graphics, PointF shoulderPos)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);

            graphics.DrawString(
                $"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}",
                new Font(SystemFonts.DefaultFont.FontFamily, 12),
                Brushes.DarkRed,
                10,
                10);
            DrawReachableZone(graphics, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);

            var windowJoints = new PointF[joints.Length + 1];
            windowJoints[0] = shoulderPos;
            var rectSize = 10;
            DrawJoint(graphics, windowJoints[0], rectSize);
            for (int i = 1; i < windowJoints.Length; i++)
            {
                windowJoints[i] = ConvertMathToWindow(joints[i - 1], shoulderPos);
                DrawLine(graphics, windowJoints[i - 1], windowJoints[i]);
                DrawJoint(graphics, windowJoints[i], rectSize);
            }
        }

        private static void DrawLine(Graphics graphics, PointF joint1, PointF joint2)
        {
            graphics.DrawLine(ManipulatorPen, joint1, joint2);
        }

        private static void DrawJoint(Graphics graphics, PointF joint, float rectSize)
        {
            graphics.FillEllipse(
                JointBrush,
                new RectangleF(joint.X - rectSize / 2, joint.Y - rectSize / 2, rectSize, rectSize));
        }

        private static void DrawReachableZone(
            Graphics graphics, 
            Brush reachableBrush, 
            Brush unreachableBrush, 
            PointF shoulderPos, 
            PointF[] joints)
		{
			var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
			var rmax = Manipulator.UpperArm + Manipulator.Forearm;
			var mathCenter = new PointF(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
			var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
			graphics.FillEllipse(reachableBrush, windowCenter.X - rmax, windowCenter.Y - rmax, 2 * rmax, 2 * rmax);
			graphics.FillEllipse(unreachableBrush, windowCenter.X - rmin, windowCenter.Y - rmin, 2 * rmin, 2 * rmin);
		}

		public static PointF GetShoulderPos(Form form)
		{
			return new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);
		}

		public static PointF ConvertMathToWindow(PointF mathPoint, PointF shoulderPos)
		{
			return new PointF(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
		}

		public static PointF ConvertWindowToMath(PointF windowPoint, PointF shoulderPos)
		{
			return new PointF(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
		}
	}
}
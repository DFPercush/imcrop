
// #if LINUX
// #else
// #define WINDOWS
// #endif

#if WINDOWS
#define USE_RECYCLE_BIN
#endif

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MathNet.Numerics.LinearAlgebra;

#if WINDOWS
using Microsoft.VisualBasic.FileIO;
#endif

namespace imcrop
{

	public partial class Form1 : Form
	{
		const int BORDER_DRAG_SIZE = 20; // pixels
#if WINDOWS
		const string PATH_SEPARATOR = "\\";
#else
		const string PATH_SEPARATOR = "/";
#endif
		readonly string[] ValidInputFileExtensions = {".jpg", ".jpeg", ".png", ".bmp", ".tif", ".tiff"};
		Matrix<float> view = CreateMatrix.Dense<float>(3, 3);
		Matrix<float> iview = CreateMatrix.Dense<float>(3, 3);
		//Matrix<float> i3 = CreateMatrix.DenseIdentity<float>(3, 3);
		Image img = null;
		//static Color grayout = Color.FromArgb(160, 108, 127, 192);
		static Color grayout = Color.FromArgb(175, 127, 127, 127);
		static Brush grayoutBrush = new SolidBrush(grayout);
		static Pen grayoutPen = new Pen(grayoutBrush);
		static Pen dottedPen = new Pen(Color.Black, 1);
		Rectangle cropArea; // image space
		Point lastMousePos = new Point();
		bool panning = false;
		bool cropping = false;
		bool rectangleSelect = false;
		Point rectStart = new Point();
		Point rectContinue = new Point();

		RectSide cropSide = RectSide.Unknown;
		string imgFilename = "";

		Stack<Rectangle> selectionHistory = new Stack<Rectangle>();


		struct SideDistance
		{
			public float distance;
			public RectSide side;
		}
		enum RectSide
		{
			Unknown,
			Left,
			Top,
			Right,
			Bottom
		};




		Vector<float> vec(Point p)
		{
			var ret = CreateVector.Dense<float>(3);
			ret[0] = p.X;
			ret[1] = p.Y;
			ret[2] = 1;
			return ret;
		}

		Point coScreenToImage(Point p)
		{
			var sco = vec3(p.X, p.Y);
			sco[2] = view[2, 2];
			var v = iview * sco; // vec(p);
			//v /= v[2];
			return new Point { X = (int)v[0], Y = (int)v[1] };
		}

		Point coImageToScreen(Point p)
		{
			var v = view * vec(p);
			//v /= v[2];
			return new Point { X = (int)v[0], Y = (int)v[1] };
		}

		//static readonly float[] fZoomInValues = { 1.1f, 0, 0, 0, 1.1f, 0, 0, 0, 1.1f };
		//static readonly Matrix<float> mZoomIn = CreateMatrix.Dense<float>(3, 3, fZoomInValues);
		//static readonly float[] fZoomOutValues = { 0.9f, 0, 0, 0, 0.9f, 0, 0, 0, 0.9f };
		//static readonly Matrix<float> mZoomOut = CreateMatrix.Dense<float>(3, 3, fZoomInValues);
		//void zoomIn()
		//{
		//	view = mZoomIn * view;
		//	iview = view.Inverse();
		//}
		//
		//void zoomOut()
		//{
		//	view = mZoomOut * view;
		//	iview = view.Inverse();
		//}

		void zoom(float factor)
		{
			//view = mat3() * factor * view;
			view = view * (mat3() * factor);
			iview = view.Inverse();
			disp.Invalidate();
		}

		void pan(float screenX, float screenY)
		{
			view[0, 2] += screenX;
			view[1, 2] += screenY;

			//var p = mat3();
			//p[0, 2] = screenX;
			//p[1, 2] = screenY;
			////view = p * view;
			//view = view * p;

			iview = view.Inverse();
			disp.Invalidate();
		}

		public Form1()
		{
			InitializeComponent();
			view = mat3();
			iview = mat3();
			//disp.MouseWheel += disp_MouseWheel;
			MouseWheel += Form_MouseWheel;
			string configPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + PATH_SEPARATOR + "imcrop.dat";
			if (File.Exists(configPath))
			{
				using (var conf = new StreamReader(configPath))
				{
					while (!conf.EndOfStream)
					{
						string line = conf.ReadLine().Trim();
						if (line.Length > 0 && line[0] != '#')
						{
							var sp = line.Split('=');
							if (sp.Length == 2)
							{
								switch (sp[0])
								{
									case "indir":
										textBox1.Text = sp[1];
										break;
									case "outdir":
										textBox2.Text = sp[1];
										break;
								}
							}
						}
					}
				}
				fileList.Focus();
			}
		}

		private void btnTestDrawAlpha_Click(object sender, EventArgs e)
		{
			//Brush b = new Brush();
			//System.Drawing.Drawing2D.
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			dottedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
			Form1_Resize(sender, e);
			fileList.Focus();
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			//var g = e.Graphics;
			//var rect = new Rectangle(100, 100, 100, 100);
			//g.FillRectangle(grayoutBrush, rect);
			//g.DrawRectangle(dottedPen, rect);
		}

		Matrix<float> mat3()
		{
			return CreateMatrix.DenseIdentity<float>(3,3);
		}

		void PopulateFiles(string pathstr)
		{
			//var dir = new Directory (pathstr);
			foreach (var file in Directory.EnumerateFiles(pathstr))
			{
				string shortname = Path.GetFileName(file);
				var fi = new FileInfo(file);
				if (ValidInputFileExtensions.Contains(fi.Extension) )
				{
					var li = new ListViewItem(shortname);
					li.Tag = file;
					li.SubItems.Add(fi.LastWriteTime.ToShortDateString() + " " + fi.LastWriteTime.ToShortTimeString());
					li.SubItems.Add(HumanReadableSize(fi.Length));
					fileList.Items.Add(li);
					//fileList.Items.Add("Hello");
				}
			}
		}

		void SetInputFolder(string path)
		{
			if (!Directory.Exists(path)) { return; }
			//fileList.Clear();
			fileList.Items.Clear();
			textBox1.Text = path;
			PopulateFiles(path);
			if (fileList.Items.Count > 0)
			{
				fileList.Items[0].Focused = true;
				fileList.Items[0].Selected = true;
			}
			textBox1.Invalidate();
			fileList.Invalidate();
		}

		private void btnBrowseSource_Click(object sender, EventArgs e)
		{
			//var fd = new FolderBrowserDialog();
			var fd = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
			var result = fd.ShowDialog();
			if (result == DialogResult.OK)
			{
				//SetInputFolder(fd.SelectedPath);
				textBox1.Text = fd.SelectedPath;
				// Rely on the text changed event to populate files
			}
		}
		private void Form1_Resize(object sender, EventArgs e)
		{
			fileList.Width = splitImageArea.Panel1.Width - (2 * fileList.Left);
			fileList.Height = splitImageArea.Panel2.Height - (2 * fileList.Top);
			int dispMaxWidth = splitImageArea.Panel2.Width - (2 * disp.Left);
			int dispMaxHeight = splitImageArea.Panel2.Height - (2 * disp.Top);
			float dispRatio = (float)dispMaxWidth / (float)dispMaxHeight;
			if (disp.Image != null)
			{
				float imageRatio = (float)disp.Image.Width / (float)disp.Image.Height;
				if (imageRatio > dispRatio)
				{
					disp.Width = dispMaxWidth;
					disp.Height = (int)(dispMaxWidth / imageRatio);
				}
				else
				{
					disp.Height = dispMaxHeight;
					disp.Width = (int)(dispMaxHeight * imageRatio);
				}
			}
			else
			{
				disp.Width = dispMaxWidth;
				disp.Height = dispMaxHeight;
			}
			disp.Invalidate();
			Invalidate();
		}

		string HumanReadableSize(long size)
		{
			if (size > 1e15)
			{
				return Math.Round((float)size / 1e15f, 0).ToString() + " PB";
			}
			if (size > 1e12)
			{
				return Math.Round((float)size / 1e12f, 0).ToString() + " TB";
			}
			if (size > 1e9)
			{
				return Math.Round((float)size / 1e9f, 0).ToString() + " GB";
			}
			if (size > 1e6)
			{
				return Math.Round((float)size / 1e6f, 0).ToString() + " MB";
			}
			if (size > 1e3)
			{
				return Math.Round((float)size / 1e3f, 0).ToString() + " KB";
			}
			return size.ToString() + " B";
		}

		private void fileList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (fileList.SelectedItems.Count > 0)
			{
				try
				{
					//disp.Image
					imgFilename = fileList.SelectedItems[0].Tag.ToString();
					using (var tmpImg = Image.FromFile(imgFilename))
					{
						img = new Bitmap(tmpImg);
					}
					// TODO: Stretch/fit
					Form1_Resize(sender, e);
					cropArea.X = 0;
					cropArea.Y = 0;
					cropArea.Width = img.Width;
					cropArea.Height = img.Height;
					selectionHistory.Clear();
					disp.Invalidate();
				}
				catch { }
			}
		}

		private void disp_Paint(object sender, PaintEventArgs e)
		{
			var g = e.Graphics;

			// Image view
			//if (disp.Image != null)
			if (img != null)
			{

				Point imgTopLeft = new Point { X = 0, Y = 0 }; // coScreenToImage(sTopLeft);
				Point imgBottomRight = new Point { X = img.Width, Y = img.Height }; //coScreenToImage(sBottomRight);
				Size imgDiffSize = new Size { Width = imgBottomRight.X - imgTopLeft.X, Height = imgBottomRight.Y - imgTopLeft.Y };

				var svTopLeft = view * vec(imgTopLeft);
				//svTopLeft /= svTopLeft[2];
				var svBottomRight = view * vec(imgBottomRight);
				//svBottomRight /= svBottomRight[2];
				Point sTopLeft = new Point { X = (int)svTopLeft[0], Y = (int)svTopLeft[1] };
				Point sBottomRight = new Point { X = (int)svBottomRight[0], Y = (int)svBottomRight[1] };
				Size sDiff = new Size { Width = sBottomRight.X - sTopLeft.X, Height = sBottomRight.Y - sTopLeft.Y };

				//Rectangle rDest = new Rectangle { X = 0, Y = 0, Width = disp.Width, Height = disp.Height };
				Rectangle rDest = new Rectangle { Location = sTopLeft, Size = sDiff };
				Rectangle rSrc = new Rectangle { Location = imgTopLeft, Size = imgDiffSize };
				//rSrc.X = Math.Max(0, imgTopLeft.X);
				//rSrc.Y = Math.Max(0, imgTopLeft.Y);
				//rSrc.Width = Math.Min(imgBottomRight.X, disp.Image.Width - rSrc.X);
				//rSrc.Height = Math.Min(imgBottomRight.Y, disp.Image.Height - rSrc.Y);
				g.DrawImage(img, rDest, rSrc, GraphicsUnit.Pixel);
			}


			Rectangle r = new Rectangle();
			var iCropTopLeft = CreateVector.Dense<float>(3);
			var iCropBottomRight = CreateVector.Dense<float>(3);
			iCropTopLeft[0] = cropArea.X;
			iCropTopLeft[1] = cropArea.Y;
			iCropTopLeft[2] = 1.0f;
			iCropBottomRight[0] = cropArea.Right;
			iCropBottomRight[1] = cropArea.Bottom;
			iCropBottomRight[2] = 1.0f;
			var sCropTopLeft = view * iCropTopLeft;
			//sCropTopLeft /= sCropTopLeft[2];
			var sCropBottomRight = view * iCropBottomRight;
			//sCropBottomRight /= sCropBottomRight[2];

			// Left gray area
			r.X = 0;
			r.Y = (int)(sCropTopLeft[1] + 0.5f); // 0;
			r.Width = (int)(sCropTopLeft[0] + 0.5f); //cropArea.Left;
			r.Height = (int)(sCropBottomRight[1] - sCropTopLeft[1] + 0.5f); //disp.Height;
			g.FillRectangle(grayoutBrush, r);

			// Top gray area
			r.X = 0;
			r.Y = 0;
			r.Width = disp.Width;
			r.Height = (int)(sCropTopLeft[1] + 0.5f); // cropArea.Height;
			g.FillRectangle(grayoutBrush, r);


			// Right gray area
			r.X = (int)(sCropBottomRight[0] + 0.5f);
			r.Y = (int)(sCropTopLeft[1] + 0.5f);
			r.Width = disp.Width; // excessive, but it will clip
			r.Height = (int)(sCropBottomRight[1] - sCropTopLeft[1] + 0.5f); //disp.Height;
			g.FillRectangle(grayoutBrush, r);
			//r.X = cropArea.
			//var m = new Matrix<float>(3, 3);


			// Bottom gray area
			r.X = 0;
			r.Y = (int)(sCropBottomRight[1] + 0.5f);
			r.Width = disp.Width;
			r.Height = disp.Height;
			g.FillRectangle(grayoutBrush, r);

			if (rectangleSelect)
			{
				Rectangle box = new Rectangle();
				box.X = Math.Min(rectStart.X, rectContinue.X);
				box.Y = Math.Min(rectStart.Y, rectContinue.Y);
				box.Width = Math.Max(rectStart.X, rectContinue.X) - box.X;
				box.Height = Math.Max(rectStart.Y, rectContinue.Y) - box.Y;
				g.DrawRectangle(Pens.Red, box);
			}
		}

		Vector<float> vec3()
		{
			var ret = CreateVector.Dense<float>(3);
			ret[2] = 1.0f;
			return ret;
		}
		Vector<float> vec3(Vector<float> v)
		{
			var ret = CreateVector.Dense<float>(3);
			ret[0] = v[0];
			ret[1] = v[1];
			if (v.Count > 2) { ret[2] = v[2]; }
			else { ret[2] = 1.0f; }
			return ret;
		}
		Vector<float> vec2(Vector<float> v)
		{
			var ret = CreateVector.Dense<float>(2);
			ret[0] = v[0];
			ret[1] = v[1];
			return ret;
		}
		Vector<float> vec2()
		{
			return CreateVector.Dense<float>(2, 0);
		}

		private void disp_MouseDown(object sender, MouseEventArgs e)
		{
			//var sco = vec3();
			//sco[0] = e.X;
			//sco[1] = e.Y;
			//var ico = iview * sco;
			Point testPoint = new Point();
			//testPoint.X = (int)ico[0];
			//testPoint.Y = (int)ico[1];

			testPoint.X = e.X;
			testPoint.Y = e.Y;
			if (e.Button == MouseButtons.Left)
			{
				var d = ClosestSide(cropArea, testPoint);
				if (d.distance * view[0, 0] <= BORDER_DRAG_SIZE)
				{
					cropping = true;
					cropSide = d.side;
					selectionHistory.Push(cropArea);
				}
				else
				{
					panning = true;
				}
			}
			else if (e.Button == MouseButtons.Middle)
			{
				panning = true;
			}
			else if (e.Button == MouseButtons.Right)
			{
				rectangleSelect = true;
				rectStart.X = e.X;
				rectStart.Y = e.Y;
				rectContinue.X = e.X;
				rectContinue.Y = e.Y;
				selectionHistory.Push(cropArea);
				disp.Invalidate();
			}
			//else if (e.Button == MouseButtons.)
			else
			{
				//MessageBox.Show($"MouseDown {((int)e.Button)}");
			}
			lastMousePos.X = e.X;
			lastMousePos.Y = e.Y;
		}

		
		private void Form_MouseWheel(object sender, MouseEventArgs e)
		{
			//int dispx = disp.Left;
			//int dispy = disp.Top;
			//
			//dispx += splitContainer1.Panel2.Left;
			//dispy += splitContainer1.Panel2.Top;
			//
			//dispx += splitContainer1.Left;
			//dispy += splitContainer1.Top;
			//
			//if (e.X >= dispx
			//	//&& e.X < disp.Right
			//	&& e.Y >= dispy
			//	//&& e.Y < disp.Bottom
			//	)
			//{
			//	var nev = new MouseEventArgs(e.Button, e.Clicks, e.X - dispx, e.Y - dispy, e.Delta);
			//	disp_MouseWheel(sender, nev);
			//}


			//Point dp = disp.PointToClient(new Point(e.X, e.Y));
			//if (dp.X >= 0 && dp.Y >= 0)
			//{
			//	disp_MouseWheel(sender, new MouseEventArgs(e.Button, e.Clicks, dp.X, dp.Y, e.Delta));
			//}


			int dispx = 0;
			int dispy = 0;
			Control tmp = disp;
			string diag = "";
			while (tmp.Parent != null)
			{
				diag += $"{tmp.GetType()} ({tmp.Left}, {tmp.Top})\r\n";
				dispx += tmp.Left;
				dispy += tmp.Top;
				tmp = tmp.Parent;
			}
			//MessageBox.Show(diag);
			if (dispx >= 0 && dispy >= 0)
			{
				disp_MouseWheel(sender, new MouseEventArgs(e.Button, e.Clicks, e.X - dispx, e.Y - dispy, e.Delta));
			}


		}
		private void disp_MouseWheel(object sender, MouseEventArgs e)
		{
			//RedirectMouse(sender, e, "wheel");
			const int WHEEL_DELTA = 120;
			pan(-e.X, -e.Y);
			zoom((float)Math.Pow(1.1f, (float)e.Delta / WHEEL_DELTA));
			//pan(e.X * 1.1f, e.Y * 1.1f);
			pan(e.X, e.Y);
		}

		Vector<float> vec3(float x, float y)
		{
			var ret = CreateVector.Dense<float>(3);
			ret[0] = x;
			ret[1] = y;
			ret[2] = 1.0f;
			return ret;
		}

		private void disp_MouseMove(object sender, MouseEventArgs e)
		{
			if (panning)
			{
				pan(e.X - lastMousePos.X, e.Y - lastMousePos.Y);
			}
			else if (rectangleSelect)
			{
				rectContinue.X = e.X;
				rectContinue.Y = e.Y;
				disp.Invalidate();
			}
			else if (cropping)
			{
				//var ico = (iview * vec3(e.X, e.Y));
				var ip = coScreenToImage(new Point(e.X, e.Y));
				//ico /= ico[2];
				//var ix = (int)ico[0];
				//var iy = (int)ico[1];
				var ix = ip.X;
				var iy = ip.Y;
				int tmp;
				// TODO: here
				switch (cropSide)
				{
					case RectSide.Left:
						tmp = cropArea.Right;
						cropArea.X = Math.Max(ix, 0);
						cropArea.Width = tmp - cropArea.X;
						break;
					case RectSide.Top:
						tmp = cropArea.Bottom;
						cropArea.Y = Math.Max(iy, 0);
						cropArea.Height = tmp - cropArea.Y;
						break;
					case RectSide.Right:
						cropArea.Width = Math.Min(ix, img.Width) - cropArea.X;
						break;
					case RectSide.Bottom:
						cropArea.Height = Math.Min(iy, img.Height) - cropArea.Y;
						break;
				}
				disp.Invalidate();
			}
			else
			{
				Point sco = new Point { X = e.X, Y = e.Y };
				var d = ClosestSide(cropArea, sco);
				var sv = vec3(e.X, e.Y);
				sv[2] = view[2, 2];
				var ico = iview * sv; // vec3(e.X, e.Y);
				//ico /= ico[2];
				//var svo = view * ico; // (ico / ico[2]);
				//svo /= svo[2];
				label1.Text = String.Format(
					"({2}, {3}) {0} {1}",
					d.distance * view[0, 0],
					d.side.ToString(),
					ico[0],
					ico[1]);
				label1.Invalidate();
				if (d.distance * view[0, 0] <= BORDER_DRAG_SIZE)
				//if (d.distance <= BORDER_DRAG_SIZE)
				{
					switch (d.side)
					{
						case RectSide.Top:
						case RectSide.Bottom:
							disp.Cursor = Cursors.SizeNS;
							//Cursor = Cursors.SizeNS;
							break;
						case RectSide.Left:
						case RectSide.Right:
							disp.Cursor = Cursors.SizeWE;
							//Cursor = Cursors.SizeWE;
							break;
						default:
							disp.Cursor = Cursors.Default;
							//Cursor = Cursors.Default;
							break;
					}
					disp.Invalidate();
				}
				else
				{
					disp.Cursor = Cursors.Default;
				}
			}




			lastMousePos.X = e.X;
			lastMousePos.Y = e.Y;
		}

		private void disp_MouseUp(object sender, MouseEventArgs e)
		{
			panning = false;
			cropping = false;
			//if (selectionHistory.Top != cropArea) { selectionHistory.Push(cropArea); }
			cropSide = RectSide.Unknown;
			disp.Cursor = Cursors.Default;
			if (rectangleSelect)
			{
				rectContinue.X = e.X;
				rectContinue.Y = e.Y;
				int tmp;
				if (rectStart.X > rectContinue.X)
				{
					tmp = rectStart.X;
					rectStart.X = rectContinue.X;
					rectContinue.X = tmp;
				}
				if (rectStart.Y > rectContinue.Y)
				{
					tmp = rectStart.Y;
					rectStart.Y = rectContinue.Y;
					rectContinue.Y = tmp;
				}


				rectStart = coScreenToImage(rectStart);
				rectContinue = coScreenToImage(rectContinue);


				cropArea.X = rectStart.X;
				cropArea.Y = rectStart.Y;
				cropArea.Width = rectContinue.X - rectStart.X;
				cropArea.Height = rectContinue.Y - rectStart.Y;


				if (cropArea.X < 0) { cropArea.X = 0; }
				if (cropArea.Y < 0) { cropArea.Y = 0; }
				if (cropArea.Right > img.Width) { cropArea.Width = img.Width - cropArea.X; }
				if (cropArea.Bottom > img.Height) { cropArea.Height = img.Height - cropArea.Y; }

				//selectionHistory.Push(cropArea);

				rectangleSelect = false;
				disp.Invalidate();
			}
			//else if (selectionHistory.Top != cropArea)
			//{
			//	selectionHistory.Push(cropArea);
			//}
			lastMousePos.X = e.X;
			lastMousePos.Y = e.Y;
			//Cursor = Cursors.Default;
		}


		SideDistance ClosestSide(Rectangle imgRect, Point screenCo)
		{

			//Vector<float> scoord =  //new Point { X = screenX, Y = screenY };
			//var scoord = CreateVector.Dense<float>(3);
			//scoord[0] = screenCo.X;
			//scoord[1] = screenCo.Y;
			//scoord[2] = 1.0f;
			//var icoord = iview * scoord;
			var ip = coScreenToImage(screenCo);
			//icoord /= icoord[2];
			//Point ip = new Point { X = (int)icoord[0], Y = (int)icoord[1] };
			Point topLeft = new Point { X = imgRect.Left, Y = imgRect.Y };
			Point bottomLeft = new Point { X = imgRect.Left, Y = imgRect.Bottom };
			Point bottomRight = new Point { X = imgRect.Right, Y = imgRect.Bottom };
			Point topRight = new Point { X = imgRect.Right, Y = imgRect.Top };

			List<SideDistance> possible = new List<SideDistance>();
			SideDistance tmp;
			tmp = new SideDistance();
			tmp.side = RectSide.Left;
			tmp.distance = DistancePointToLineSegment(topLeft, bottomLeft, ip);
			possible.Add(tmp);

			tmp = new SideDistance();
			tmp.side = RectSide.Right;
			tmp.distance = DistancePointToLineSegment(topRight, bottomRight, ip);
			possible.Add(tmp);

			tmp = new SideDistance();
			tmp.side = RectSide.Top;
			tmp.distance = DistancePointToLineSegment(topLeft, topRight, ip);
			possible.Add(tmp);

			tmp = new SideDistance();
			tmp.side = RectSide.Bottom;
			tmp.distance = DistancePointToLineSegment(bottomLeft, bottomRight, ip);
			possible.Add(tmp);

			possible.Sort((SideDistance a, SideDistance b) => (int)(a.distance - b.distance));
			return possible[0];
		}

		float Distance(Vector<float>a, Vector<float> b)
		{
			return (float)MathNet.Numerics.Distance.Euclidean<float>(a, b);
		}
		float Magnitude(Vector<float> v)
		{
			Vector<float> z = CreateVector.Dense<float>(2, 0);
			return Distance(z, v);
		}

		float DistancePointToLineSegment(Point A, Point B, Point P)
		{
			var a = CreateVector.Dense<float>(2);
			a[0] = A.X;
			a[1] = A.Y;
			var b = CreateVector.Dense<float>(2);
			b[0] = B.X;
			b[1] = B.Y;
			var p = CreateVector.Dense<float>(2);
			p[0] = P.X;
			p[1] = P.Y;
			var ab = b - a;
			var ap = p - a;
			var bp = p - b;
			if (ab.DotProduct(ap) < 0)
			{
				// point a
				return (float)Math.Sqrt((ap[0] * ap[0]) + (ap[1]*ap[1]));
			}
			if (ab.DotProduct(bp) > 1)
			{
				// point b
				return (float)Math.Sqrt((bp[0] * bp[0]) + (bp[1] * bp[1]));
			}

			//var dotNorm = ab.Normalize(2).DotProduct(ap.Normalize(2));
			var dotNorm = ab.DotProduct(ap) / Magnitude(ab); // / Magnitude(ap) ;
			//var mag = Magnitude(ab);
			//var fromANorm = dot / mag;
			
			//var fromANorm = ab.DotProduct(ap) / Magnitude(ab);
			var intersect = a + (ab.Normalize(2) * dotNorm);
			return Distance(p, intersect);

			//float smap = Magnitude(ap);
			//smap *= smap;
			//float smab = Magnitude(ab);
			//smab *= smab;
			//return (float)Math.Sqrt(Math.Abs(smap - smab));
		}

		private void splitImageArea_SplitterMoved(object sender, SplitterEventArgs e)
		{
			Form1_Resize(sender, e);
		}

		int timer1Counter = 0;
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (timer1Counter++ % 2 == 1)
			{
				disp.Cursor = Cursors.SizeNS;
			}
			else
			{
				disp.Cursor = Cursors.SizeWE;
			}
		}

		private void disp_Click(object sender, EventArgs e)
		{
		}

		string timestamp()
		{
			var t = DateTime.Now;
			return t.Year.ToString("D4")
				+ t.Month.ToString("D2")
				+ t.Day.ToString("D2")
				+ "-"
				+ t.Hour.ToString("D2")
				+ t.Minute.ToString("D2")
				+ t.Second.ToString("D2");
		}

		void Save(ImageFormat format, string extensionWithDot)
		{
			if (!Directory.Exists(textBox2.Text)) { return; }
			Image cropped = new Bitmap(cropArea.Width, cropArea.Height); //, GraphicsUnit.Pixel);
			using (var g = Graphics.FromImage(cropped))
			{
				Rectangle destRect = new Rectangle();
				destRect.X = 0;
				destRect.Y = 0;
				destRect.Width = cropped.Width;
				destRect.Height = cropped.Height;
				g.DrawImage(img, destRect, cropArea, GraphicsUnit.Pixel);
			}
			string srcFilename = imgFilename; // fileList.SelectedItems[0].Tag.ToString();
			var srcinf = new FileInfo(srcFilename);
			string destFilename = textBox2.Text
				+ PATH_SEPARATOR
				//+ Path.GetFileNameWithoutExtension(fileList.SelectedItems[0].Tag.ToString());
				+ Path.GetFileNameWithoutExtension(imgFilename);
				
			if (File.Exists(destFilename + extensionWithDot))
			{
				destFilename += "_" + timestamp();
			}
			
			
			cropped.Save(destFilename + extensionWithDot, format);

			File.SetCreationTime(destFilename + extensionWithDot, srcinf.CreationTime);
			if (extensionWithDot == ".png")
			{
				// TODO: pngout
			}


			rm(srcFilename);
			popFileList();


		}

		void popFileList()
		{
			//fileList.Items.Remove(fileList.SelectedItems[0]);
			int lastSelection = 0;
			for (int i = 0; i < fileList.Items.Count; i++)
			{
				if (fileList.Items[i].Tag.ToString() == imgFilename)
				{
					fileList.Items.RemoveAt(i);
					lastSelection = i;
					break;
				}
			}

			if (fileList.Items.Count > lastSelection)
			{
				fileList.Items[lastSelection].Focused = true;
				fileList.Items[lastSelection].Selected = true;
			}
			else if (fileList.Items.Count > 0)
			{
				fileList.Items[0].Focused = true;
				fileList.Items[0].Selected = true;
			}
			else
			{
				img = null;
				imgFilename = "";
				cropArea.X = 0;
				cropArea.Y = 0;
				cropArea.Width = 0;
				cropArea.Height = 0;
				disp.Invalidate();
			}
		}

		private void btnPNG_Click(object sender, EventArgs e)
		{
			Save(ImageFormat.Png, ".png");
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			SetInputFolder(textBox1.Text);
		}

		private void btnBrowseDest_Click(object sender, EventArgs e)
		{
			var fd = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
			if (fd.ShowDialog() == DialogResult.OK)
			{
				textBox2.Text = fd.SelectedPath;
			}
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			string datPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + PATH_SEPARATOR + "imcrop.dat";
			using (var dat = new StreamWriter(datPath))
			{
				dat.WriteLine("indir=" + textBox1.Text);
				dat.WriteLine("outdir=" + textBox2.Text);
			}
		}

		private void btnJpg_Click(object sender, EventArgs e)
		{
			Save(ImageFormat.Jpeg, ".jpg");
		}

		
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (fileList.SelectedItems.Count == 0) { return; }
			//string filename = fileList.SelectedItems[0].Tag.ToString();
			rm(imgFilename);
			popFileList();
		}

		void rm(string filename)
		{
#if USE_RECYCLE_BIN
			//System.IO.FileSystem
			Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
				filename,
				UIOption.OnlyErrorDialogs,
				RecycleOption.SendToRecycleBin);
#else
			File.Delete(filename);
#endif
		}

		private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			//if (textBox1.Focused) { return; }
			//if (textBox2.Focused) { return; }
			if (e.KeyCode == Keys.J) { btnJpg_Click(sender, e); }
			else if (e.KeyCode == Keys.P) { btnPNG_Click(sender, e); }
			else if (e.KeyCode == Keys.Delete) { btnDelete_Click(sender, e); }
			else if (e.KeyCode == Keys.D) { btnDuplicate_Click(sender, e); }
			else if (e.KeyCode == Keys.O)
			{
				// Original
				if (imgFilename.ToLower().EndsWith(".png"))
				{
					btnPNG_Click(sender, e);
				}
				else if (imgFilename.ToLower().EndsWith(".jpg") || imgFilename.EndsWith(".jpeg"))
				{
					btnJpg_Click(sender, e);
				}
			}
			else if (e.KeyCode == Keys.M)
			{
				btnMove_Click(sender, e);
			}
			else if (e.KeyCode == Keys.B)
			{
				if (selectionHistory.Count >= 1)
				{
					//selectionHistory.Pop();
					cropArea = selectionHistory.Pop();
					disp.Invalidate();
				}
			}
			else
			{
				//MessageBox.Show($"KeyDown {((int)e.KeyCode)}");

			}
		}

		private void btnMove_Click(object sender, EventArgs e)
		{
			//imgFilename
			string destFilename = textBox2.Text
				+ PATH_SEPARATOR
				+ Path.GetFileNameWithoutExtension(imgFilename);
			string ext = Path.GetExtension(imgFilename);
			if (File.Exists(destFilename + ext))
			{
				destFilename += "_" + timestamp();
			}
			destFilename += ext;

			File.Move(imgFilename, destFilename);
			popFileList();

		}

		class FileListComparer : System.Collections.IComparer
		{
			public delegate int CompareLambda(ListView L, ListViewItem a, ListViewItem b);
			ListView list;
			CompareLambda fn;
			public FileListComparer(ListView listObject, CompareLambda compareFunc)
			{
				list = listObject;
				fn = compareFunc;
			}
			//public int Compare(object x, object y)
			public int Compare(object a, object b)
			{
				return fn(list, a as ListViewItem, b as ListViewItem);
			}
		};

		public static long HumanReadableSizeToLong(string h)
		{
			if (h.EndsWith("PB")) { return (long)1e15 * long.Parse(h.Substring(0, h.Length - 2)); }
			if (h.EndsWith("TB")) { return (long)1e12 * long.Parse(h.Substring(0, h.Length - 2)); }
			if (h.EndsWith("GB")) { return (long)1e9 * long.Parse(h.Substring(0, h.Length - 2)); }
			if (h.EndsWith("MB")) { return (long)1e6 * long.Parse(h.Substring(0, h.Length - 2)); }
			if (h.EndsWith("KB")) { return 1000 * long.Parse(h.Substring(0, h.Length - 2)); }
			if (h.EndsWith("B")) { return long.Parse(h.Substring(0, h.Length - 2)); }
			else { return long.Parse(h.Substring(0, h.Length - 2)); }
		}

		bool comparersInitialized = false;
		System.Collections.IComparer byName;
		System.Collections.IComparer byDate;
		System.Collections.IComparer bySize;


		private void fileList_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (!comparersInitialized)
			{
				byName =
					new FileListComparer(fileList, (ListView L, ListViewItem a, ListViewItem b) =>
					(L.Sorting == SortOrder.Descending ? -1 : 1) *
					String.Compare(a.Text, b.Text));
				byDate =
					new FileListComparer(fileList, (ListView L, ListViewItem a, ListViewItem b) =>
					(L.Sorting == SortOrder.Descending ? -1 : 1) * 
					DateTime.Parse(a.SubItems[1].Text).CompareTo(DateTime.Parse(b.SubItems[1].Text)));
				bySize =
					new FileListComparer(fileList, (ListView L, ListViewItem a, ListViewItem b) =>
					(L.Sorting == SortOrder.Descending ? -1 : 1) * 
					Math.Sign(HumanReadableSizeToLong(a.SubItems[2].Text) - HumanReadableSizeToLong(b.SubItems[2].Text)));
				comparersInitialized = true;
			}

			if (fileList.Sorting == SortOrder.Ascending) { fileList.Sorting = SortOrder.Descending; }
			else { fileList.Sorting = SortOrder.Ascending; }
			if (e.Column == 0)
			{
				fileList.ListViewItemSorter = byName;
			}
			else if (e.Column == 1)
			{
				fileList.ListViewItemSorter = byDate;
			}
			else if (e.Column == 2)
			{
				fileList.ListViewItemSorter = bySize;
			}
			fileList.Sort();
		}

		private void btnDuplicate_Click(object sender, EventArgs e)
		{
			string destFilename = ((int)1).ToString("D3");
			for (int n = 0; n <= 999; n++)
			{
				
				destFilename = Path.GetDirectoryName(imgFilename)
					+ "\\"
					+ Path.GetFileNameWithoutExtension(imgFilename)
					+"-"
					+ n.ToString("D3")
					+ Path.GetExtension(imgFilename);
				if (!File.Exists(destFilename)) { break; }
			}

			File.Copy(imgFilename, destFilename);
			var imginf = new FileInfo(imgFilename);
			File.SetCreationTime(destFilename, imginf.CreationTime);
			File.SetLastAccessTime(destFilename, imginf.LastAccessTime);

			string shortname = Path.GetFileName(destFilename);
			var li = new ListViewItem(shortname);
			li.Tag = destFilename;
			var fi = new FileInfo(destFilename);
			li.SubItems.Add(fi.LastWriteTime.ToShortDateString() + " " + fi.LastWriteTime.ToShortTimeString());
			li.SubItems.Add(HumanReadableSize(fi.Length));
			var index = (fileList.SelectedIndices.Count > 0) ? fileList.SelectedIndices[0] : 0;
			fileList.Items.Insert(index, li);
			for (int i = 0; i < fileList.Items.Count; i++)
			{
				fileList.Items[i].Selected = false;
				fileList.Items[i].Focused = false;
			}
			fileList.Items[index].Focused = true;
			fileList.Items[index].Selected = true;

		}
	}
}

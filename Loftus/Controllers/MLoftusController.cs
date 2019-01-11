using Loftus.DBS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ImageMagick;
using Loftus.Models;
using System.Web.Security;

namespace Loftus.Controllers
{
	public class MLoftusController : Controller
	{

		public MLoftusController()
		{
		}

		ArtDBS dbs = new ArtDBS();

		public static string RandomString(int length)
		{
			Random random = new Random();
			const string chars = "a0b0c0d0i0o0v0w0x0y0z0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static void ProcessFile(string path)
		{
			using (MagickImage image = new MagickImage(path))
			{
				string newFile = path.Replace(Path.GetExtension(path), ".jpg");
				image.Write(newFile);
			}
		}

		private bool isValidContentType(string contentType)
		{

			return contentType.Equals("image/png")
				|| contentType.Equals("image/gif")
				|| contentType.Equals("image/jpg")
				|| contentType.Equals("application/octet-stream")
				|| contentType.Equals("image/jpeg");
		}


		private void CheckAdmin()
		{

			if (Session["admin"] == null)
			{
				Session["admin"] = false;
			}
		}



		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		static Size GetThumbnailSize(Image original)
		{
			// Maximum size of any dimension.
			Random r = new Random();
			int maxPixels = r.Next(400, 700);
			//const int maxPixels = 500;

			// Width and height.
			int originalWidth = original.Width;
			int originalHeight = original.Height;

			// Compute best factor to scale entire image based on larger dimension.
			double factor;
			if (originalWidth > originalHeight)
			{
				factor = (double)maxPixels / originalWidth;
			}
			else
			{
				factor = (double)maxPixels / originalHeight;
			}

			// Return thumbnail size.
			return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
		}



		public ActionResult Welcome()
		{
			CheckAdmin();
			return View("Welcome");
		}

		public ActionResult Login()
		{
			CheckAdmin();
			return View();
		}

		public ActionResult AddPhoto()
		{
			CheckAdmin();
			return View();
		}

		[HttpPost]
		public ActionResult Update(string title, string year, string dems, string medium, string category, string addInfo, int id)
		{
			int rowAffected = dbs.EditArtwork(title, year, dems, medium, category, addInfo, id);
			string status = "Error";

			if (rowAffected > 0)
			{
				status = "Update Successful!";
			}

			//return View("EditDetails");
			return Json(status, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Process(HttpPostedFileBase photo, string title, string year, string dems, string medium, string category, string addInfo)
		{
			string salt = $"{RandomString(12)}_";
			if (title == null)
			{
				title = photo.FileName;
			}
			string pathConst;

			if (photo != null)
			{
				if (!isValidContentType(photo.ContentType))
				{
					return View("Index");
				}
				else
				{
					var path = Path.Combine(Server.MapPath("~/Photos"), salt + photo.FileName);
					pathConst = path;
					photo.SaveAs(path);

					if (photo.ContentType.Equals("application/octet-stream"))
					{
						using (MagickImage image = new MagickImage(path))
						{
							string newFile = path.Replace(Path.GetExtension(path), ".JPG");
							int newArtID = dbs.AddArtwork(title, Path.GetFileName(newFile), year, dems, medium, category, addInfo);
							pathConst = newFile;
							image.Write(newFile);
						}
						System.IO.File.Delete(path);
					}
					else
					{
						int newArtID = dbs.AddArtwork(title, salt + photo.FileName, year, dems, medium, category, addInfo);
					}

					ImageOptimizer opt = new ImageOptimizer();
					opt.Compress(pathConst);

					Image img = Image.FromFile(pathConst);
					Size thumbnailSize = GetThumbnailSize(img);

					using (MagickImage image = new MagickImage(pathConst))
					{
						string newfilename = Path.GetFileName(pathConst);
						image.Resize(thumbnailSize.Width, thumbnailSize.Height);
						string thumbPath = Path.Combine(Server.MapPath("~/Photos/Thumbs"), newfilename);
						image.Write(thumbPath);
					}
					img.Dispose();

					return Redirect(Request.UrlReferrer.ToString());
				}
			}
			return Redirect(Request.UrlReferrer.ToString());
		}


		[HttpGet]
		public ActionResult GetPics()
		{

			var photos = dbs.GetAllArt();
			return Json(photos, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetDetails()
		{
			var photos = dbs.GetAllArt();
			return Json(photos, JsonRequestBehavior.AllowGet); ;
		}

		public ActionResult EditDetails()
		{
			if (Session["admin"] == null)
			{
				Session["admin"] = false;
			}
			if ((bool)Session["admin"] == true)
			{
				return View();
			}
			else
			{
				return RedirectToAction("Welcome", "mloftus");
			}
		}

		[HttpPost]
		public ActionResult Mla(string email, string password)
		{
			string invalid = "Sorry, username or passwork are incorrect.";
			User user = dbs.GetUser(email,password);

			if (email == "" || email == null || password == "" || password == null || user == null)
			{
				TempData["invalid"] = invalid;
				return RedirectToAction("Login");
				//return View("Login", (object)invalid);
			}

			if (user.Password == password && user.Email == email)
			{
				FormsAuthentication.SetAuthCookie(user.Email, true);
				Session["email"] = user.Email;

				if (user.IsAdmin == true)
				{
					Session["admin"] = true;
				}
				else
				{
					Session["admin"] = false;
				}

				Session["LoggedIn"] = true;

				return RedirectToAction("EditDetails", "Mloftus");
			}
			else
			{
				TempData["invalid"] = invalid;
				return RedirectToAction("Login");
				//return View("Login", (object)invalid);
			}

		}

		public ActionResult Logout()
		{
			Session["admin"] = false;
			return View("Welcome");
		}





	}
}
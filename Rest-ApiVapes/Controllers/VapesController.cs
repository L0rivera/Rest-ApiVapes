using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Rest_ApiVapes.Data;
using Rest_ApiVapes.Models;
using Rest_ApiVapes.utils;

namespace Rest_ApiVapes.Controllers
{
    public class VapesController : ControllerBase
    {
        private readonly VapeData _vapeData;
        private readonly CloudinaryService _cloudinary;

        public VapesController(VapeData vapeData, CloudinaryService cloudinary)
        {
            _vapeData = vapeData;
            _cloudinary = cloudinary;
        }

        // GET: Vape
        [HttpGet]
        [Route("api/vapes")]
        public IEnumerable<Vape> Get()
        {
            return _vapeData.GetVapes();
        }

        [HttpPost]
        [Route("api/vape")]
        public async Task<IActionResult> Post([FromForm] Vape vape, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("No file uploaded", nameof(image));
            }

            try
            {
                var UploadResult = await _cloudinary.UploadImageAsync(image);

                var Vape = new Vape
                {
                    name = vape.name,
                    description = vape.description,
                    price = vape.price,
                    status = vape.status,
                    brand = vape.brand,
                    flavor = vape.flavor,
                    public_id = UploadResult.PublicId,
                    secure_url = UploadResult.SecureUrl,
                    nicotine = vape.nicotine,
                    e_liquid = vape.e_liquid,
                    mAh = vape.mAh,
                    uses = vape.uses,
                    rechargable = vape.rechargable
                };

                 _vapeData.addVape(Vape);

                return CreatedAtAction(nameof(Get), new { id = vape.VapeId }, vape);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}

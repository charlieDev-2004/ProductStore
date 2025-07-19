using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PictureController(IRepository<Picture> pictureRepository, IWebHostEnvironment webHostEnvironment)
        {
            _pictureRepository = pictureRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture([FromRoute] int id)
        {
            try
            {
                var spec = new PictureSpecification(id);
                var picture = await _pictureRepository.GetById(spec);

                if (picture == null)
                    return BadRequest(new { Message = $"La imagen con el id {id} no existe" });

                var webRootPath = _webHostEnvironment.WebRootPath;

                if (System.IO.File.Exists(Path.Combine(webRootPath, picture.Path)))
                {
                    System.IO.File.Delete(Path.Combine(webRootPath, picture.Path));
                    await _pictureRepository.Delete(picture);
                }

                 return Ok(new { Message = "Imagen eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ha ocurrido un error", ErrorMessage = ex.Message });
            }
        }
    }
}
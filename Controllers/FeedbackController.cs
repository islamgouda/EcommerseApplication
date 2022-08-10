using EcommerseApplication.DTO;
using EcommerseApplication.Models;
using EcommerseApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly Ifeedback feedbackrepository;
        private readonly IProductRepository productrepository;
        private readonly ConsumerRespons Respons;
        private readonly Ipartener partnerrepository;

        public FeedbackController(Ifeedback feedbackrepository,IProductRepository productrepository,ConsumerRespons respons,Ipartener partnerrepository)
        {
            this.feedbackrepository = feedbackrepository;
            this.productrepository = productrepository;
            this.Respons = respons;
            this.partnerrepository = partnerrepository;
        }
        [HttpGet("getbyproductId")]
        public IActionResult GetFeedbackByProductID(int Id)
        {
            Product product = productrepository.Get(Id);
            if (product != null)
            {
                try
                {
                    List<feedback> feedbackList = feedbackrepository.getByfeedbackProductID(Id);
                    if (feedbackList.Count>0)
                    {
                        List<FeedBackDTO> feedbackListdto = new List<FeedBackDTO>();
                        foreach(feedback feedback in feedbackList)
                        {
                            FeedBackDTO feed = new FeedBackDTO();
                            feed.UserID=feedback.UserID;
                            feed.Comment=feedback.Comment;
                            feed.OrderID=feedback.OrderID;
                            feed.Rate=feedback.Rate;
                            feedbackListdto.Add(feed);
                        }
                        Respons.succcess = true;
                        Respons.Message = " get feedback Done";
                        Respons.Data = feedbackListdto;
                        return Ok(Respons);
                      
                    }
                    else
                    {
                        Respons.Message = "no feedback for this product";
                        Respons.succcess = false;
                        Respons.Data = "";
                        return BadRequest(Respons);
                        
                    }
                }
                catch (Exception ex)
                {
                    Respons.Message = ex.InnerException.Message;
                    Respons.succcess = false;
                    Respons.Data = "";
                    return BadRequest(Respons);
                }
            }
            Respons.Message = "this product not found";
            Respons.succcess = false;
            Respons.Data = "";
            return BadRequest(Respons);
        }
        [HttpGet("getfeedParttnerBypartnerId")]
        public IActionResult getrateofpartner(int Id)
        {
            Partener partener=partnerrepository.getByID(Id);
            if (partener != null)
            {
                try
                {
                    int rate = (int)feedbackrepository.getratepartnerbyId(Id);
                    if (rate > 0)
                    {
                        Respons.succcess = true;
                        Respons.Message = " get Rate of Partner Done";
                        rate = (rate * 100) / 100;
                        Respons.Data = (rate)+"%";
                        return Ok(Respons);
                        
                    }
                    else
                    {
                        Respons.Message = "no exists rate for this partner";
                        Respons.succcess = false;
                        Respons.Data = "";
                        return BadRequest(Respons);
                      
                    }
                }
                catch(Exception ex)
                {
                    Respons.Message = ex.InnerException.Message;
                    Respons.succcess = false;
                    Respons.Data = "";
                    return BadRequest(Respons);
                
                }
            }
            Respons.Message = "not exists this partner ";
            Respons.succcess = false;
            Respons.Data = "";
            return BadRequest(Respons);
        }
    }
    
}

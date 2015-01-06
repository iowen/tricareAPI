using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TriCareAPI.Utilities;

namespace TriCareAPI.Controllers
{
    //[Authorize]
    public class IngredientController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var util = new IngredientUtil(new TriCareDataDataContext());
            var result = util.ConvertListToModel(util.GetIngredients());
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // GET api/values/5
        public string Get(int id)
        {
            var util = new IngredientUtil(new TriCareDataDataContext());
            var result = util.ConvertToModel(util.GetIngredient(id));
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // POST api/values
        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<Ingredient>(value);
            result.IngredientId = 0;
            var util = new IngredientUtil(new TriCareDataDataContext());
            var outPut = util.CreateIngredient(result);
            var json = new JavaScriptSerializer().Serialize(outPut);
            return json;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

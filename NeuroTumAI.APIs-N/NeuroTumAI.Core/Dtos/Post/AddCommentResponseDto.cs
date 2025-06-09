using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class AddCommentResponseDto
	{
        public string Comment { get; set; }
        public int PostId { get; set; }
    }
}

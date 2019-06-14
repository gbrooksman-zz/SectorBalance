using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using SectorBalanceClient.SVG;
using Microsoft.AspNetCore.Blazor.Rendering;

namespace SectorBalanceClient
{
    public class HeadingComponentBase : ComponentBase 
    {      

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;

            builder.OpenElement(seq, "figure");
                builder.OpenElement(++seq, "div");
                    //builder.AddAttribute(++seq, "class", "doughnut-main");

                    builder.OpenElement(++seq, "svg");
                        builder.AddAttribute(++seq, "width", "100%");
                        builder.AddAttribute(++seq, "height", "100%");
                        builder.AddAttribute(++seq, "viewBox", "0 0 420 420");
                            builder.OpenElement(++seq, "circle");
                                builder.AddAttribute(++seq, "cx", 50);
                                builder.AddAttribute(++seq, "cy",50);
                                builder.AddAttribute(++seq, "r", 20);
                                builder.AddAttribute(++seq, "stroke", "black");
                                builder.AddAttribute(++seq, "stroke-width", 3);
                                builder.AddAttribute(++seq, "fill", "red");            
                                builder.CloseElement();
                    builder.CloseElement();
                builder.CloseElement();
            builder.CloseElement();
        }

    }
}

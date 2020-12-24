using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace XUnitTests
{
    public class MathNetSandbox
    {

        [Fact]
        public void DoSomethingOne()
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,]
            {
                {1, 1, 1 },
                {1,2,3 },
                {6,4,2 }
            });

            var x = A.Add(1.0);



        }

    }


}

using Data_capture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_capture.Controllers
{
    public class CusMath
    {
        public static List<decimal[]> getCalculatedSingleList(List<Measurement> tempDataCaptureContext)
        {
            List<decimal[]> tempList = new();

            int listCount = tempDataCaptureContext.Count;

            decimal[] tempTemperature = new decimal[listCount];
            decimal[] tempHumidity = new decimal[listCount];
            decimal[] tempWeight = new decimal[listCount];
            decimal[] tempWidth = new decimal[listCount];
            decimal[] tempLength = new decimal[listCount];
            decimal[] tempDepth = new decimal[listCount];

            for (int i = 0; i < tempDataCaptureContext.Count; i++)
            {
                tempTemperature[i] = tempDataCaptureContext[i].MTemperature;
                tempHumidity[i] = tempDataCaptureContext[i].MHumidity;
                tempWeight[i] = tempDataCaptureContext[i].MWeight;
                tempWidth[i] = tempDataCaptureContext[i].MWidth;
                tempLength[i] = tempDataCaptureContext[i].MLength;
                tempDepth[i] = tempDataCaptureContext[i].MDepth;
            }

            tempList.Add(tempTemperature);
            tempList.Add(tempHumidity);
            tempList.Add(tempWeight);
            tempList.Add(tempWidth);
            tempList.Add(tempLength);
            tempList.Add(tempDepth);

            return tempList;

        }

        public static decimal getHighest(decimal[] inList)
        {
            return inList.Max();
        }

        public static decimal getLowest(decimal[] inList)
        {
            return inList.Min();
        }

        public static decimal getMean(decimal[] inList)
        {
            decimal countTotal = 0;

            for (int i = 0; i < inList.Length; i++)
            {
                countTotal += inList[i];
            }

            decimal mean = countTotal / inList.Count();

            return mean;
        }
        
        public static decimal getVariance(decimal[] inList)
        {
            /*
             * 1 Mean
             * 2 every element - mean
             * 3 Square every element from 2
             * 4 Add squared numbers
             * 5 Divide the sum of squares by n – 1 or N. #(sum of squares) / ( (total num of squares) -1)
             */

            decimal tempMean = getMean(inList);//1

            decimal totalSquaredNumbers = 0;//3

            for (int i = 0; i < inList.Length; i++)
            {
                decimal tempNumLessMean = inList[i] - tempMean;//2
                totalSquaredNumbers += tempNumLessMean * tempNumLessMean;//4
            }

            return totalSquaredNumbers / inList.Count() - 1;//5
        }

        public static decimal getSum(decimal[] inList) 
        {
            return inList.Sum();
        }
        
        public static decimal getStandardDeviation(decimal[] inList) 
        {
            //1.Work out the Mean(the simple average of the numbers)
            //2.Then for each number: subtract the  and Mean square the result
            //3.Then work out the mean of those squared differences.
            //4.Take the square root of that and we are done!

            decimal tempMean = getMean(inList);//1

            for (int i = 0; i < inList.Length; i++)
            {
                decimal tempNumLessMean = inList[i] - tempMean;//2
                inList[i] = tempNumLessMean * tempNumLessMean;//4
            }

            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(getMean(inList))));
        }



    }
}

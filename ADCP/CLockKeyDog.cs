using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aladdin.HASP;

namespace ADCP
{
    public class CLockKeyDog
    {
        public bool UnLockKeyDog(int ID)
        {
            HaspFeature feature = HaspFeature.FromFeature(ID);

            string vendorCode =
            "Ddscp3Z7hp33spdnO+mbH2Guf/e171WtGtCgrc/674B8xPGrBKyESHpO6DOWt/zFJXrYFDiNa6AyiBmU" +
            "bVYG85VOCJcTeVPbjyWx7mA6q3vtKm+ZrP8zTIVrhayli+oORs6XcWoyXHSaTWeplzXvMjw/9ZMyApPa" +
            "F2uKYBEUF3ioCW7NEYF/DW3dz26xzf9ArwW7d7/YXNcKXfuRDmxaNvCyg/XHxCKjvtfiFFU4tn+HGWWn" +
            "qem3e9jVd+RydBQF28xrTN78PXozAq4qNA7F2RN1IgGvnq5PXMTIgUZS79YFCmmu7T3YRZYPl8YHSccz" +
            "MSrEHpL65fyFuyyaSDBU1B2Ztl3fR+8gHyxU4tEg80Ek3F7G8Kh/ZcvXv91D1L4uyftttTe+1VOIiJBv" +
            "BQSyXiU6Sefp4E0kH5n+sUXq+g2xhisG/z7UwHWY3Vp4TfDyRCPEQPw+HC9xw7CaT6oOBGOBVTHkzgsh" +
            "hBI3+KFuiXDHoQglHd0URRSCGxNOs9OsTqr2oXbKv2gqM9E+Leto0kCIMKB97Bhnha98M64WoiAXXbZR" +
            "x+7kOlEc1Dw5JKdUubK7jBUiUTNQD+4uMz2P38EC5IS6yPD4Ly6wFw/xdK4ej6NmZ/Sqsrg0kp+j7NPk" +
            "/+gpuJooKvXLZWvwBG7pnUVvY/rDFrc5fBJQH6DnKrt1XtJNizc+PbE04HT5YWlDA7Vgsn0L5SQucW8L" +
            "XIcVovbpNxZj9qcNREnVbOGZ4CjLMNuQmBO6CyeB2pPFFffchiZf2ZYeuTJ19J3obn8dyiAcHlVsJEIJ" +
            "4WNE9MA6/QDCWeI5OBQVEeQX3fBlgh26prXoEf2yyS5XC1iMCwDxWWF85sTU5tlhts7jyuzLyiza+JCY" +
            "k6OVeFKfFDH2g19XkwHe9iZ/W+zRYVokGkomrqV/AjyZZi+kcyUwYDeIlyU=";

            Hasp hasp = new Hasp(feature);
            HaspStatus status = hasp.Login(vendorCode);
            if (HaspStatus.StatusOk != status)
            {
                return false;
            }
            else
            {
                return true;
            }

           
        }
    }
}

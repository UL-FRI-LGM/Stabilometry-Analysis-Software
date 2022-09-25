namespace StabilometryAnalysis
{
    public class CMatrix
    {
        public float Cxx = 0f;
        public float Cxy = 0f;
        public float Cyy = 0f;

        public CMatrix(float Cxx, float Cxy, float Cyy)
        {
            this.Cxx = Cxx;
            this.Cxy = Cxy;
            this.Cyy = Cyy;
        }
    }
}
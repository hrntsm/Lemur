namespace Lemur.Control.Solver
{
    public enum LeSolverMethod
    {
        CG = 0,
        BiCGSTAB = 1,
        GMRES = 2,
        GPBiCG = 3,
        DIRECT = 10,
        DIRECTmkl = 11,
        MUMPS = 12
    }
}

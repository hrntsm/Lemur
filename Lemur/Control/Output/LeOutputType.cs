using System;

namespace Lemur.Control.Output
{
    [Serializable]
    public enum LeOutputType
    {
        DISP,
        ROT,
        REACTION,
        NSTRAIN,
        NSTRESS,
        NMISES,
        ESTRAIN,
        ESTRESS,
        EMISES,
        ISTRAIN,
        ISTRESS,
        PL_ISTRAIN,
        VEL,
        ACC,
        TEMP,
        PRINC_NSTRESS,
        PRINCV_NSTRESS,
        PRINC_NSTRAIN,
        PRINCV_NSTRAIN,
        PRINC_ESTRESS,
        PRINCV_ESTRESS,
        PRINC_ESTRAIN,
        PRINCV_ESTRAIN,
        SHELL_LAYER,
        SHELL_SURFACE,
        CONTACT_NFORCE,
        CONTACT_FRICTION,
        CONTACT_RELVEL,
        CONTACT_STATE,
        CONTACT_NTRACTION,
        CONTACT_FTRACTION,
    }
}

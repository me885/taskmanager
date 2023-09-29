import { Button } from '@mui/material';
import { useNavigate } from "react-router-dom";

const SigninButton = () => {   
    const navigate = useNavigate(); 
    
    const handleClick = () => {
        navigate("/signin");
    }

    return (
        <Button 
            id="signin-button"
            className="navbar-signin"
            size="small"
            variant="contained"
            onClick={handleClick}
        >
            Sign in
        </Button>
    )
}

export default SigninButton;
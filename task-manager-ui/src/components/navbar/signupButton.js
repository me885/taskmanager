import { Button } from '@mui/material';
import { useNavigate } from "react-router-dom";

const SignupButton = () => {   
    const navigate = useNavigate(); 
    
    const handleClick = () => {
        navigate("/signup");
    }

    return (
        <Button 
            id="signup-button"
            className="navbar-signup"
            size="small"
            variant="outlined"
            onClick={handleClick}
        >
            Sign up
        </Button>
    )
}

export default SignupButton;
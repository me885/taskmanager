import { Button } from '@mui/material';
import { useNavigate } from "react-router-dom";

const LogoutButton = () => {   
    const navigate = useNavigate(); 
    
    const handleClick = () => {
        localStorage.removeItem("jwt")
        navigate("/signin");
    }

    return (
        <Button 
            id="logout-button"
            className="navbar-logout"
            size="small"
            variant="contained"
            onClick={handleClick}
        >
            Logout
        </Button>
    )
}

export default LogoutButton;
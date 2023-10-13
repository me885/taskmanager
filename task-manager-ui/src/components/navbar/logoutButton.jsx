import { Button } from '@mui/material';
import { useContext } from 'react';
import { useNavigate } from "react-router-dom";
import UserContext from '../../UserContext';

const LogoutButton = () => {
    const navigate = useNavigate(); 
    const {setLoggedInState} = useContext(UserContext)
    
    const handleClick = () => {
        localStorage.removeItem("jwt")
        setLoggedInState(false)
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
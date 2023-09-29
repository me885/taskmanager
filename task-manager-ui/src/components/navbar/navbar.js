import { Box } from '@mui/material';
import SigninButton from './signinButton';
import SignupButton from './signupButton';
import "./navbar.css";
import { Router } from 'react-router-dom';



const Navbar = () => {   
    return (
        <Box className="navbar">
            <Box className="left-content">
                TaskManager
            </Box>

            <Box className="right-content">
                <SignupButton />
                <SigninButton />
            </Box>
        </Box>
    )
}

export default Navbar;
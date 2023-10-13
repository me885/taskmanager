import { Box } from '@mui/material';
import SigninButton from './signinButton';
import SignupButton from './signupButton';
import LogoutButton from './logoutButton';

import "./navbar.css";


const Navbar = () => {   
    return (
        <Box className="navbar">
            <Box className="left-content">
                TaskManager
            </Box>

            <Box className="right-content">
                <LogoutButton />
                <SignupButton />
                <SigninButton />
            </Box>
        </Box>
    )
}

export default Navbar;
import {Button, Container, TextField, Stack} from '@mui/material';
import { useNavigate } from "react-router-dom";
import "./signup.css";
import UserContext from '../UserContext';
import { useContext } from 'react';


const SignUp = () => {  

    const {setLoggedInState} = useContext(UserContext)
    
    const navigate = useNavigate(); 
    const handleExisitingAccount = () =>
    {
        navigate("/signup");
    }

    const handleSubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);

        const loginObject = {
            name: formData.get("username"),
            password: formData.get("password"),
        }

        await fetch("https://taskmanager-todo.azurewebsites.net/auth/getToken", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify(loginObject),
        })
        .catch(error => {
            console.log(error)
            console.log("failed");
        })
        .then(async (response) => {
            if(response.status === 200)
            {
                localStorage.setItem("jwt", (await response.json()).token);
                setLoggedInState(true);
                document.getElementById("login-form").reset()
                navigate("/");
            }
        })
    };
  
    return (
        <Container className="signup-box">
            <Stack
            id="login-form"
            className="signup-stack"
            component="form"
            onSubmit={handleSubmit}
            spacing={2}
            noValidate
            autoComplete="off">
                <TextField 
                    id="username" 
                    label="Username" 
                    name="username"
                    variant="outlined"
                    className="signup-textbox" />
                <TextField 
                    id="password" 
                    label="Password" 
                    name="password"
                    variant="outlined"
                    type="password" 
                    className="signup-textbox" />
                <Button 
                    className="signup-button"
                    type="submit"
                    variant="contained"
                    color="primary">
                    Login
                </Button>
                <Button 
                    className="signup-button"
                    onClick={handleExisitingAccount}
                    variant="outlined"
                    color="primary">
                    Create an account
                </Button>
            </Stack>
      </Container>
    )
}

export default SignUp;
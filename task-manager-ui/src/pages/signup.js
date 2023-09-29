import {Button, Container, TextField, Stack} from '@mui/material';
import { useNavigate } from "react-router-dom";
import "./signup.css";


const SignUp = () => {  

    const navigate = useNavigate(); 
    const handleExisitingAccount = () =>
    {
        navigate("/signin");
    }

    const handleSubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);

        const signupObject = {
            name: formData.get("username"),
            password: formData.get("password"),
        }

        console.log(signupObject);

        await fetch("https://taskmanager-todo.azurewebsites.net/auth/register", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify(signupObject),
            mode: 'no-cors'
        })
        .then(async () => {
            document.getElementById("signup-form").reset()

            await fetch("https://taskmanager-todo.azurewebsites.net/auth/getToken", {
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(signupObject),
                mode: 'no-cors'
            })
            .then(response => {
                console.log(response.body)
            })
        })
        .catch(error => {
            console.log(error)
            console.log("failed");
        })
    };
  
    return (
        <Container className="signup-box">
            <Stack
            id="signup-form"
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
                    Create Account
                </Button>
                <Button 
                    className="signup-button"
                    onClick={handleExisitingAccount}
                    variant="outlined"
                    color="primary">
                    I already have an account
                </Button>
            </Stack>
      </Container>
    )
}

export default SignUp;
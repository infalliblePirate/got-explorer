import { Link, Navigate, useNavigate } from "react-router-dom";
import "./Auth.scss";
import { useState } from "react";
import Cookies from "universal-cookie";
import authService from "./authService";
import { toast } from "sonner";
import { GoogleLogin } from "@react-oauth/google";
import ErrorHandle from "../../utils/ErrorHandle";


const SignUpPage = () => {
    const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%]).{8,24}$/;
    const EML_REGEX = /^[\w-\\.]+@([\w-]+\.)+[\w-]{2,4}$/g;
    const navigate = useNavigate();

    const authserv = authService;
    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null;

    const [userData, setUserData] = useState({
        username: "",
        email: "",
        password: ""
    });

    const [isPasswordVisible, setPasswordVisible] = useState(false);

    const togglePasswordVisibility = () => {
        setPasswordVisible(!isPasswordVisible);
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setUserData({
            ...userData,
            [e.target.name]: value
        });
    };

    const SubmitGoogle = (token: string) => {
        authserv.login_google(token)
            .then(() => {
                toast.success("Successful registration!", {
                    style: {
                        backgroundColor: '#cfc15d',
                        color: 'white'
                    }
                });
                navigate("/startgame");
            })
            .catch((error) => {
                toast.error(`Registration failed: ${ErrorHandle(error.response.data.errors)}`, {
                    style: {
                        backgroundColor: '#5d8ecf',
                        color: 'white'
                    }
                });
            });
    }

    const Submit = () => {
        const passValid = PWD_REGEX.test(userData.password);
        const emailValid = EML_REGEX.test(userData.email);

        if (passValid && emailValid && userData.username !== "") {
            authserv.signup(userData.username, userData.email, userData.password)
                .then(() => {
                    toast.success("Successful registration!", {
                        style: {
                            backgroundColor: '#cfc15d',
                            color: 'white'
                        }
                    });
                    navigate("/startgame");
                })
                .catch((error) => {
                    toast.error(`Registration failed: ${ErrorHandle(error.response.data.errors)}`, {
                        style: {
                            backgroundColor: '#5d8ecf',
                            color: 'white'
                        }
                    });
                });
        } else {
            if (!passValid) {
                toast.error("Password should contain 1 uppercase letter; 1 lowercase letter; 1 digit; 1 special symbol", {
                    style: {
                        backgroundColor: '#5d8ecf',
                        color: 'white'
                    }
                });
            }
            if (!emailValid) {
                toast.error("Email is invalid", {
                    style: {
                        backgroundColor: '#5d8ecf',
                        color: 'white'
                    }
                });
            }
            if (userData.username === "") {
                toast.error("Username is empty", {
                    style: {
                        backgroundColor: '#5d8ecf',
                        color: 'white'
                    }
                });
            }
        }
    };

    return (
        <>
            {isAuthenticated ? <Navigate to="/" /> :
                <div className="auth-grid">
                    <img className="photo-bg" />
                    <div className="col-2">
                        <img className="logo" />
                        <div className="greeting">Nice to see you</div>
                        <form>
                            <label className="register-label" htmlFor="username">Name</label>
                            <input id="username" name="username" placeholder="Username" autoComplete="off" required value={userData.username} onChange={handleChange} />
                            <label className="register-label" htmlFor="email">Login</label>
                            <input id="email" name="email" placeholder="Email" autoComplete="off" value={userData.email} onChange={handleChange} />
                            <label className="register-label" htmlFor="password">Password</label>
                            <div id="pass-container">
                                <input id="password" name="password" type={isPasswordVisible ? 'text' : 'password'} placeholder="Password" value={userData.password} onChange={handleChange} />
                                <button type="button" onClick={togglePasswordVisibility}>
                                    <img id="pass-eye-img" />
                                </button>
                            </div>
                            <input className="submit-btn" type="button" value="Sign Up" onClick={Submit}></input>
                        </form>
                        <Link to="/login" className="link">Already have an account? Sign in</Link>
                        <hr></hr>
                        <div className="google">
                            <GoogleLogin
                                onSuccess={r => {
                                    SubmitGoogle(r.credential as string);
                                }}
                                onError={() => {
                                    toast.error("Gmail login failed", {
                                        style: {
                                            backgroundColor: '#5d8ecf',
                                            color: 'white'
                                        }
                                    });
                                }}
                            />
                        </div>
                    </div>
                </div>
            }
        </>
    );
};

export default SignUpPage;

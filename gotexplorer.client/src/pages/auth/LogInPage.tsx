import { Link, Navigate, useNavigate } from "react-router-dom";
import "./Auth.scss";
import { useState } from "react";
import authService from "./authService";
import Cookies from "universal-cookie";
import { toast } from 'sonner';
import { GoogleLogin } from "@react-oauth/google";


const LogInPage = () => {
    const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%]).{8,24}$/;
    const [userData, setUserData] = useState({
        username: "",
        password: ""
    });
    const navigate = useNavigate();

    const authserv = authService;
    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null ? true : false;


    const [isPasswordVisible, setPasswordVisible] = useState(false);
    const [rememberMe, setRememberMe] = useState(false);

    const togglePasswordVisibility = () => {
        setPasswordVisible(!isPasswordVisible);
    };

    const toggleRememberMe = () => {
        setRememberMe(!rememberMe);
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
                console.error("Registration failed:", error);
                const errorMsg = error.response?.data?.errors[0]?.errorMessage || "An error occurred.";
                toast.error(errorMsg, {
                    style: {
                        backgroundColor: '#5d8ecf',
                        color: 'white'
                    }
                });
            });
    }

    const submit = () => {
        const passValid = PWD_REGEX.test(userData.password);


        if (passValid && userData.username !== "") {
            authserv.login(userData.username, userData.password, rememberMe)
                .then(() => {
                    toast.success("Login successful!", {
                        style: {
                            backgroundColor: '#cfc15d',
                            color: 'white'
                        }
                    });
                    navigate("/startgame");
                })
                .catch((error) => {
                    console.error("Login failed:", error);
                    const errorMsg = error.response?.data?.errors[0]?.errorMessage || "An error occurred.";
                    toast.error(errorMsg, {
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
                        <div className="greeting">
                            Nice to see you
                        </div>
                        <form>
                            <label>Login</label>
                            <div id="email-container">
                                <input
                                    id="username"
                                    name="username"
                                    placeholder="Username"
                                    required
                                    autoComplete="off"
                                    value={userData.username}
                                    onChange={handleChange} />
                            </div>
                            <label>Password</label>
                            <div id="pass-container">
                                <input
                                    id="pass"
                                    name="password"
                                    type={isPasswordVisible ? 'text' : 'password'}
                                    placeholder="Password"
                                    required
                                    autoComplete="off"
                                    value={userData.password}
                                    onChange={handleChange} />
                                <button type="button" onClick={togglePasswordVisibility}>
                                    <img id="pass-eye-img" />
                                </button>
                            </div>
                            <div id="rmmbr-btn-container">
                                <label className="remember-me">
                                    <input
                                        type="checkbox"
                                        checked={rememberMe}
                                        onChange={toggleRememberMe}
                                    />
                                    Remember me
                                </label>
                                <Link to="/forgetpass" className="link"> Forgot password </Link>
                            </div>

                            <input className="submit-btn" type="button" value="Log in" onClick={submit}></input>
                        </form>
                        <Link to="/signup" className="link">Don't have an account? Sign up</Link>
                        <hr></hr>
                        <div className="google">
                            <GoogleLogin
                                onSuccess={r => {
                                    SubmitGoogle(r.credential as string);
                                }}
                                onError={() => {
                                    console.log('Login Failed');
                                }}
                            />
                        </div>
                    </div>
                </div>
            }
        </>
    );
};

export default LogInPage;


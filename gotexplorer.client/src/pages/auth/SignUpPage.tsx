import { Link, Navigate, useNavigate } from "react-router-dom";
import "./Auth.scss";
import { useState } from "react";
import Cookies from "universal-cookie";
import authService from "./authService";

const SignUpPage = () => {
    const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%]).{8,24}$/;
    const EML_REGEX = /^[\w-\\.]+@([\w-]+\.)+[\w-]{2,4}$/g;
    const navigate = useNavigate();

    const authserv = authService;
    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null ? true : false;

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
    function Submit() {
        const passValid = PWD_REGEX.test(userData.password);
        const emailValid = EML_REGEX.test(userData.email);
        if (passValid && emailValid) {
            authserv.signup(userData.username, userData.email, userData.password)
                .then((r) => {
                    console.log("Registration successful:", r);
                    navigate("/");
                })
                .catch((error) => {
                    console.error("Registration failed:", error);
                });
        }
        else {
            if (!passValid) {
                console.log('wrong pass');
            }
            if (!emailValid) {
                console.log('wrong email');
            }
        }
        console.log(cookies.get('token'));
    }
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
                            <label className="register-label" htmlFor="username">Name</label>
                            <input id="username"
                                name="username"
                                placeholder="Username"
                                autoComplete="off"
                                required
                                aria-aria-describedby=""
                                defaultValue={userData.username}
                                onChange={handleChange} />
                            <label className="register-label" htmlFor="email">Login</label>
                            <input id="email"
                                name="email"
                                placeholder="Email"
                                autoComplete="off"
                                defaultValue={userData.email}
                                onChange={handleChange} />
                            <label className="register-label" htmlFor="password">Password</label>
                            <div id="pass-container">
                                <input id="password" name="password"
                                    type={isPasswordVisible ? 'text' : 'password'}
                                    placeholder="Password"
                                    defaultValue={userData.password}
                                    onChange={handleChange} />
                                <button type="button" onClick={togglePasswordVisibility}>
                                    <img id="pass-eye-img" />
                                </button>
                            </div>
                            <input className="submit-btn" type="button" value="Sign Up" onClick={Submit}></input>
                        </form>
                        <Link to="/login" className="link">Already have an account? Sign in</Link>
                    </div>
                </div>
            }
        </>
    );
}
export default SignUpPage;
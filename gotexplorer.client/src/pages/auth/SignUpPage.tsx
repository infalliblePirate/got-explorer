import { Link } from "react-router-dom";
import "./Auth.scss";
import { useState } from "react";
const SignUpPage = () => {
    const [isPasswordVisible, setPasswordVisible] = useState(false);

    const togglePasswordVisibility = () => {
        setPasswordVisible(!isPasswordVisible);
    };
    return (
        <div className="auth-grid">
            <img className="photo-bg" />
            <div className="col-2">
                <img className="logo" />
                <div className="greeting">
                    Nice to see you
                </div>
                <form>
                    <label>Name</label>
                    <input id="username" placeholder="Username" />
                    <label>Login</label>
                    <input id="email" placeholder="Email" />
                    <label>Password</label>
                    <div id="pass-container">
                        <input id="pass"
                            type={isPasswordVisible ? 'text' : 'password'} placeholder="Password" />
                        <button type="button" onClick={togglePasswordVisibility}>
                            <img id="pass-eye-img" />
                        </button>
                    </div>
                    <input type="submit" value="Sign Up"></input>
                </form>
                <Link to="/login" className="link">Already have an account? Sign in</Link>
            </div>
        </div>
    );
}
export default SignUpPage;
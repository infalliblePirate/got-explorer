import Footer from '../additional_components/Footer';
import Navigation from '../additional_components/Navigation';
import './ProfilePage.scss';
import profileIcon from '../../assets/images/profile-icon.png';

const ProfilePage = () => {
     return (
         <div className="profile-page">
            <Navigation/>
             <div className="profile-container">
                 <div className="email">
                     <p>youremail@example.com</p>
                 </div>
                <div className="profile-header">
                    <div className="profile-picture-wrapper">
                        <img
                            className="profile-picture"
                            src={profileIcon}
                            alt="Profile"
                        />
                        <button className="change-picture-button">📸</button>
                    </div>
                    <h2 className="username">username</h2>
                </div>

                <div className="leaderboard">
                    <h4>Leader board</h4>
                    <p>
                        1. User23 - 1984 points
                        <br />
                        34. You - 578 points
                    </p>
                </div>
                 
                 
                <div className="profile-form">
                    <div className="form-group">
                        <label htmlFor="name">Name</label>
                        <input id="name" type="text" placeholder="Change your name" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="password">Password</label>
                        <input id="password" type="password" placeholder="Change password" />
                    </div>
                     <button className="save-button">Save changes</button>
                    <div className="two-buttons">
                        <button className="logout-button">Log out</button>
                        <button className="delete-button">Delete account</button>
                    </div>
                </div>
            </div>
            <Footer/>
        </div>
    );
}
export default ProfilePage;
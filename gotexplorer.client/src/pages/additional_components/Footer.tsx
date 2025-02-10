import { Link } from "react-router-dom";

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer__socials">
        <a href="https://www.facebook.com/dte.apeps.kpi" target="_blank" rel="noopener noreferrer">
          <i className="fab fa-facebook"></i>
        </a>
        <a href="https://github.com/infalliblePirate/got-explorer" target="_blank" rel="noopener noreferrer">
          <i className="fab fa-github"></i>
        </a>
        <a href="https://t.me/presinfokpi" target="_blank" rel="noopener noreferrer">
          <i className="fab fa-telegram"></i>
        </a>
        <a href="https://www.instagram.com/kpiuaofficial/" target="_blank" rel="noopener noreferrer">
          <i className="fab fa-instagram"></i>
        </a>
      </div>
      <div className="footer__links">
        <Link to="/#info-section">GOT world</Link>
        <Link to="/#info-section">The game</Link>
        <Link to="/#team-section">Team</Link>
      </div>
      <div className="footer__copyright">
        © 2024 All Rights Reserved
      </div>
    </footer>
  );
};

export default Footer;

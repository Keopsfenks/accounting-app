import React from 'react';
import * as PropTypes from "prop-types";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGooglePlusG, faFacebookF} from '@fortawesome/free-brands-svg-icons';

FontAwesomeIcon.propTypes = {icon: PropTypes.string};
const Login = () => {
	return (
		<div className="sign-in">
			<div className="header">
				<h1>Yeniden Hoşgeldiniz!</h1>
				<p>
					Hesabınız yok mu? <a href="#">Hesap Oluştur</a>.
				</p>
			</div>
			<form>
				<div className="social-icons">
					<a href="#" className="icon">
						<FontAwesomeIcon icon={faGooglePlusG} />
					</a>
					<a href="#" className="icon">
						<FontAwesomeIcon icon={faFacebookF} />
					</a>
					<p>ile giriş yapabilirsiniz.</p>
				</div>
				<div className="input-container">
					<div className="form-group">
						<label htmlFor="email">E-Posta:</label>
						<input type="email" className="email-input" id="email"/>
					</div>

					<div className="form-group">
						<label htmlFor="password">Şifre:</label>
						<input type="password" className="password-input" id="password"/>
					</div>
				</div>
				<div className="form-checkbox">
					<input
						type="checkbox"
						id="remember"
						name="remember"
						className="form-check-input"
					/>
					<label htmlFor="remember" className="form-check-label">
						Beni hatırla
					</label>
					<div className={"recaptcha"}></div>
				</div>
				<button type={"submit"} className={"cool-btn"} name={"login-button"}>Oturum Aç</button>
			</form>
		</div>
	);
};

export default Login;
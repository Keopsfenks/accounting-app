import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGooglePlusG, faFacebookF} from '@fortawesome/free-brands-svg-icons';

const Register = () => {
	return (
		<div className="sign-up">
			<div className="header">
				<h1>Hoşgeldiniz</h1>
				<p>
					Hesabınız var mı? <a href={"#"}>Oturum Aç!</a>
				</p>
			</div>
			<form>
				<div className="social-icons">
					<a href="#" className="icon">
						<FontAwesomeIcon icon={faGooglePlusG}/>
					</a>
					<a href="#" className="icon">
						<FontAwesomeIcon icon={faFacebookF}/>
					</a>
					<p>ile hesap oluşturabilirsiniz.</p>
				</div>
				<div className="input-container">
					<div className="group-form">
						<div className="form-group">
							<label htmlFor="name">İsminiz:</label>
							<input type="text" className="name-input" id="name"/>
						</div>
						<div className="form-group">
							<label htmlFor="surname">Soyadınız:</label>
							<input type="text" className="surname-input" id="surname"/>
						</div>
					</div>
					<div className="form-group">
						<label htmlFor="email">E-Posta:</label>
						<input type="email" className="email-input" id="email"/>
					</div>

					<div className="form-group">
						<label htmlFor="password">Şifre:</label>
						<input type="password" className="password-input" id="password"/>
					</div>

					<div className="form-group">
						<label htmlFor="repassword">Şifre (Tekrar):</label>
						<input type="password" className="repassword-input" id="repassword"/>
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
				<button type={"submit"} className={"cool-btn"} name={"login-button"}>Hesap Oluştur</button>
			</form>
		</div>
	);
};

export default Register;
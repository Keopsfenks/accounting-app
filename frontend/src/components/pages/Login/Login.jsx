import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGooglePlusG, faFacebookF, faGithub, faLinkedinIn, faWhatsapp } from '@fortawesome/free-brands-svg-icons';
import "./login.css"

const Login = () => {
	return (
		<section className={"container"}>
			<div className={"form-container sign-in"}>
				<h1>Yeniden Hoşgeldiniz!</h1>
				<p>
					Hesabınız yok mu? <a href={""}>Hesap Oluştur</a>
				</p>
				<form>
					<div className={"social-icons"}>
						<a href="#" className="icon">
							<FontAwesomeIcon icon={faGooglePlusG}/>
						</a>
						<a href="#" className="icon">
							<FontAwesomeIcon icon={faFacebookF}/>
						</a>
						<p> İle Giriş yapabilirsiniz</p>
					</div>
					<input type={"email"} id={"email"} placeholder={"E-Posta"}/>
					<input type={"password"} id={"password"} placeholder={"Şifreniz"}/>
					<div className={"form-check"}>
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
			<div className={"form-container sign-up"} style={{display: "none"}}>
				<h1>Hoşgeldiniz!</h1>,
				<p>
					Hesabınız var mı? <a href={"#"}>Oturum Aç!</a>
				</p>
				<form>
					<div className={"social-icons"}>
						<a href="#" className="icon">
							<FontAwesomeIcon icon={faGooglePlusG}/>
						</a>
						<a href="#" className="icon">
							<FontAwesomeIcon icon={faFacebookF}/>
						</a>
					</div>
					<input type={"text"} id={"name"} placeholder={"Adınız:"}/>
					<input type={"text"} id={"surname"} placeholder={"Soyadınız:"}/>
					<input type={"email"} id={"email"} placeholder={"E-Posta"}/>
					<input type={"password"} id={"password"} placeholder={"Şifreniz"}/>
					<input type={"password"} id={"repassword"} placeholder={"Şifreniz (Tekrar)"}/>
					<div className={"form-check"}>
						<input
							type="checkbox"
							id="remember"
							name="remember"
							className="form-check-input"
						/>
						<label htmlFor="remember" className="form-check-rules">
							<a href={"#"}>Kullanım Koşullarını</a> ve <a href={"#"}>Gizlilik Sözleşmesini</a> okudum,
							kabul ediyorum.
						</label>
						<div className={"recaptcha"}></div>
						<button type={"submit"} className={"cool-btn"} name={"login-button"}>Oturum Aç</button>
					</div>
				</form>
			</div>
			<hr/>
			<p className={"help-parag"}>Yardıma ihtiyacınız mı var bize aşağıdaki bağlantılardan
				ulaşabilirsiniz.</p>
			<div className="whatsapp-btn cool-btn">
				<a href={"#"} className={"icon"}>
					<FontAwesomeIcon icon={faWhatsapp}></FontAwesomeIcon> Whatsapp
				</a>
			</div>
		</section>
	);
};

export default Login;